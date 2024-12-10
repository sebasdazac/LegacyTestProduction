using LegacyTest.Models;
using LegacyTest.Models.Request;
using LegacyTest.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NuGet.Protocol.Plugins;

public class EpaycoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;


    private readonly IConfiguration _configuration;
    private readonly LegacyDBContext _context;

    public EpaycoController(IHttpClientFactory httpClientFactory, LegacyDBContext context, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        this._context = context;
        this._configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetCurrentPlan()
    {
        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));

        var currentPlan = _context.PlanCompanies
            .Where(pc => pc.IsActive == true && pc.DateInitial <= DateTime.Now && pc.DateEnd >= DateTime.Now
            && pc.IdCompany == idCompany)
            .OrderByDescending(pc => pc.DateInitial)
            .Select(pc => new
            {
                pc.IdPlanNavigation.NamePlan,
                pc.IdPlanNavigation.Price,
                pc.IdPlanNavigation.Description,
                pc.IdPlanNavigation.Bonus,
                pc.IdPlanNavigation.LimitAccount,
                StartDate = pc.DateInitial,
                EndDate = pc.DateEnd
            })
            .FirstOrDefault();

        if (currentPlan == null)
        {
            // Valores por defecto si no hay un plan activo y vigente
            return Json(new
            {
                NamePlan = "Sin Plan Activo",
                Price = 0,
                Description = "Actualmente no tienes un plan activo.",
                Bonus = "N/A",
                LimitAccount = 0,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            });
        }

        return Json(currentPlan);
    }


    public IActionResult VerifyPlanStatus()
    {
        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
        var currentDate = DateTime.Today;

        var mostRecentPlan = _context.PlanCompanies
            .Include(pc => pc.IdPlanNavigation)
            .Where(pc => pc.IdCompany == idCompany)
            .OrderByDescending(pc => pc.DateEnd)
            .ThenByDescending(pc => pc.Id)
            .FirstOrDefault();

        if (mostRecentPlan == null)
        {
            return Ok(new { message = "No tienes planes asignados.", status = "no_plan" });
        }

        if (mostRecentPlan.DateInitial <= currentDate && mostRecentPlan.DateEnd >= currentDate)
        {
            if (mostRecentPlan.IsActive == true)
            {
                return Ok(new
                {
                    planName = mostRecentPlan.IdPlanNavigation.NamePlan,
                    expiryDate = mostRecentPlan.DateEnd.ToString("yyyy-MM-dd"),
                    status = "active_and_valid"
                });
            }
            else
            {
                return Ok(new
                {
                    planName = mostRecentPlan.IdPlanNavigation.NamePlan,
                    expiryDate = mostRecentPlan.DateEnd.ToString("yyyy-MM-dd"),
                    status = "valid_but_inactive"
                });
            }
        }
        else
        {
            return Ok(new 
            {
                planName = mostRecentPlan.IdPlanNavigation.NamePlan, 
                status = "no_valid_plan"
            });
        }
    }



    public IActionResult EpaycoResponse(string ref_payco)
    {
        ViewBag.RefPayco = ref_payco;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EpaycoConfirmation([FromForm] PaymentConfirmationModel paymentData)
    {

        string p_cust_id_cliente = _configuration["Epayco:CustomerID"];
        string p_key = _configuration["Epayco:ApiKey"];


        string signatureToValidate = GenerateSignature(
            p_cust_id_cliente,
            p_key,
            paymentData.x_ref_payco,
            paymentData.x_transaction_id,
            paymentData.x_amount,
            paymentData.x_currency_code
        );

        if (paymentData.x_signature == signatureToValidate)
        {

            switch (int.Parse(paymentData.x_cod_transaction_state))
            {
                case 1:
                    await UpdateOrCreateTransaction(paymentData, "Aprobado");
                    await UpgradePlan(paymentData);
                    break;
                case 2:
                    await UpdateOrCreateTransaction(paymentData, "Rechazado");
                    break;
                case 3:
                    await UpdateOrCreateTransaction(paymentData, "Pendiente");
                    break;
                case 4:
                    await UpdateOrCreateTransaction(paymentData, "Fallida");
                    break;
                default:
                    return BadRequest("Estado de transacción no reconocido.");
            }
            return Ok("Confirmación de pago procesada correctamente.");
        }
        else
        {
            return BadRequest("Firma no válida.");
        }
    }

    private string GenerateSignature(string customerID, string apiKey, string refPayco, string transactionID, string amount, string currencyCode)
    {
        string data = $"{customerID}^{apiKey}^{refPayco}^{transactionID}^{amount}^{currencyCode}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder hash = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }
    }

    private async Task UpdateOrCreateTransaction(PaymentConfirmationModel paymentData, string transactionState)
    {
        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
        var transaction = new TransactionCompany
        {
            IdCompany = idCompany,
            IdPlanCompany = long.Parse(paymentData.x_extra1),
            Price = double.Parse(paymentData.x_amount),
            DateTransaction = DateTime.UtcNow,
            StateTransaction = transactionState,
            NumberReference = paymentData.x_ref_payco,
            Currency = paymentData.x_currency_code,
            PaymentForm = paymentData.x_response,
            CodeNameBank = paymentData.x_approval_code,
            PaymentPlataform = paymentData.x_bank_name,
            CodeTraceability = paymentData.x_id_invoice
        };

        _context.TransactionCompanies.Add(transaction);
        await _context.SaveChangesAsync();
    }


    private async Task UpgradePlan(PaymentConfirmationModel paymentData)
    {
        long idPlanCompany = long.Parse(SessionHelper.GetValue(User, "IdPlanCompany"));
        long idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
        List<string> errors = new List<string>();

        var parameters = new[]
        {
            new SqlParameter("@IdPlanCompany", SqlDbType.BigInt) { Value = idPlanCompany },
            new SqlParameter("@IdCompany", SqlDbType.BigInt) { Value = idCompany},
            new SqlParameter("@IdPlanNew", SqlDbType.BigInt) { Value = Int32.Parse(paymentData.x_extra1) },                        
            new SqlParameter("@Retorno", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
        };

        await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[UpgradePlanCompany] @IdPlanCompany, @IdCompany, @IdPlanNew, @Retorno OUT", parameters);

        var procedureOutput = (string)parameters.Last().Value;

        var planCompany = await _context.PlanCompanies.FirstOrDefaultAsync(x => x.IdCompany ==idCompany && x.IsActive == true);
        await UpdatePlanClaims(planCompany.Id, Int32.Parse(paymentData.x_extra1));

        if (procedureOutput.StartsWith("False:"))
        {
            errors.Add(procedureOutput.Substring(6));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactionStatus(string ref_payco)
    {
        string url = $"https://secure.epayco.co/validation/v1/reference/{ref_payco}";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            JObject transactionData = JObject.Parse(responseContent);

            if (transactionData["success"].Value<bool>())
            {
                var transactionDetails = transactionData["data"];


                string x_ref_payco = transactionDetails["x_ref_payco"]?.ToString();
                string x_transaction_id = transactionDetails["x_transaction_id"]?.ToString();
                string x_amount = transactionDetails["x_amount"]?.ToString();
                string x_currency_code = transactionDetails["x_currency_code"]?.ToString();
                string x_id_invoice = transactionDetails["x_id_invoice"]?.ToString();
                string x_approval_code = transactionDetails["x_approval_code"]?.ToString();
                string x_bank_name = transactionDetails["x_bank_name"]?.ToString();
                string x_payment_method = transactionDetails["x_payment_method"]?.ToString();
                string x_response = transactionDetails["x_response"]?.ToString();
                string x_response_reason_text = transactionDetails["x_response_reason_text"]?.ToString();
                string x_description = transactionDetails["x_description"]?.ToString();
                string x_cod_response = transactionDetails["x_cod_response"]?.ToString();
                string x_extra1 = transactionDetails["x_extra1"]?.ToString();


                var paymentData = new PaymentConfirmationModel
                {
                    x_ref_payco = x_ref_payco,
                    x_transaction_id = x_transaction_id,
                    x_amount = x_amount,
                    x_currency_code = x_currency_code,
                    x_id_invoice = x_id_invoice,
                    x_approval_code = x_approval_code,
                    x_bank_name = x_bank_name,
                    x_payment_method = x_payment_method,
                    x_response = x_response,
                    x_response_reason_text = x_response_reason_text,
                    x_extra1 = x_extra1
                };
                string message;

                switch (x_cod_response)
                {
                    case "1":
                        message = "¡Transacción Aprobada!";
                        await UpdateOrCreateTransaction(paymentData, "Aprobado");
                        await UpgradePlan(paymentData);
                        break;
                    case "2":
                        message = "Transacción Rechazada";
                        await UpdateOrCreateTransaction(paymentData, "Rechazado");
                        break;
                    case "3":
                        message = "Transacción Pendiente";
                        await UpdateOrCreateTransaction(paymentData, "Pendiente");
                        break;
                    case "4":
                        message = "Transacción Fallida";
                        await UpdateOrCreateTransaction(paymentData, "Fallida");
                        break;
                    default:
                        message = "Estado de la transacción no reconocido";
                        return BadRequest("Estado de transacción no reconocido.");
                }

                return Ok(new
                {
                    success = true,
                    message = message,
                    transactionDetails = new
                    {
                        x_ref_payco,
                        x_transaction_id,
                        x_amount,
                        x_currency_code,
                        x_id_invoice,
                        x_approval_code,
                        x_bank_name,
                        x_payment_method,
                        x_response,
                        x_response_reason_text,
                        x_description,
                        x_cod_response
                    }
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al consultar la transacción."
                });
            }
        }
        else
        {
            return StatusCode((int)response.StatusCode, new
            {
                success = false,
                message = "No se pudo obtener la transacción de ePayco."
            });
        }
    }

    public class PaymentConfirmationModel
    {
        public string x_ref_payco { get; set; }
        public string x_transaction_id { get; set; }
        public string x_cod_transaction_state { get; set; } // Estado del pago
        public string x_amount { get; set; }
        public string x_currency_code { get; set; }
        public string x_signature { get; set; }
        public string x_response { get; set; }
        public string x_response_reason_text { get; set; }
        public string x_id_invoice { get; set; }
        public string x_approval_code { get; set; }
        public string x_bank_name { get; set; } // Banco, si aplica
        public string x_payment_method { get; set; } // Método de pago (Efecty, Baloto, etc.)
        public string x_extra1 { get; set; } // Campo personalizado 1
    }


    public async Task<IActionResult> UpdatePlanClaims(long newIdPlanCompany, long newIdPlan)
    {
        var user = HttpContext.User;
        if (user.Identity.IsAuthenticated)
        {
            // Remueve las claims anteriores
            var identity = (ClaimsIdentity)user.Identity;

            var oldIdPlanCompanyClaim = identity.FindFirst("IdPlanCompany");
            var oldIdPlanClaim = identity.FindFirst("IdPlan");

            if (oldIdPlanCompanyClaim != null)
            {
                identity.RemoveClaim(oldIdPlanCompanyClaim);
            }

            if (oldIdPlanClaim != null)
            {
                identity.RemoveClaim(oldIdPlanClaim);
            }

            // Agrega las nuevas claims
            identity.AddClaim(new Claim("IdPlanCompany", newIdPlanCompany.ToString()));
            identity.AddClaim(new Claim("IdPlan", newIdPlan.ToString()));

            // Crea un nuevo principal con las claims actualizadas
            var principal = new ClaimsPrincipal(identity);

            // Refresca el ticket de autenticación
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddDays(1),
                    IsPersistent = true
                });

            return Json(new { success = true, message = "Claims actualizadas exitosamente" });
        }
        return Json(new { success = false, errorMessage = "No se pudo actualizar las claims" });
    }

}
