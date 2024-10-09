using LegacyTest.Models;
using LegacyTest.Tools;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Utils;
using RazorLight;
using System.Text;

namespace LegacyTest.Controllers
{
  public class RegisterController : Controller
  {
        private readonly IConfiguration _configuration;
        private readonly LegacyDBContext _context;
        private readonly Crypto crypto = new();


        public RegisterController(LegacyDBContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        private static string GenerateCompanyFree(string nombreBase)
        {
            string fechaActual = DateTime.Now.ToString("yyyyMMdd_HHmm");
            return $"{nombreBase}_{fechaActual}";
        }
        private static string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var token = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                int randomIndex = random.Next(chars.Length);
                token.Append(chars[randomIndex]);
            }

            return token.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPerson([FromBody] Person model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingUser = _context.People.FirstOrDefault(p => p.Email.ToUpper() == model.Email.ToUpper());
                    if (existingUser != null)
                    {
                        return BadRequest("Registro de usuario invalido");
                    }

                    model.Token = GenerateToken();
                    model.IdRole = 3;
                    model.Pswd = crypto.Encrypt(model.Pswd);

                    var company = new Company
                    {
                        BusinessName = $"Empresa {GenerateCompanyFree(model.Token)}",
                        CommercialReg = $"Empresa {GenerateCompanyFree(model.Token)}",
                        TypeReg = "NO DATA"
                    };

                    _context.Companies.Add(company);

                    await _context.SaveChangesAsync(); // Guarda el registro temporalmente


                    var planComany = new PlanCompany
                    {
                        IdCompany = company.Id,
                        IdPlan = 1,
                        DateInitial = DateTime.Now,
                        DateEnd = DateTime.Now.AddMonths(3),
                        IsActive = true
                    };

                    _context.PlanCompanies.Add(planComany);

                    await _context.SaveChangesAsync(); // Guarda el registro temporalmente

                    model.IdCompany = company.Id; // Asigna el Id de la compañía creada al modelo de persona
                    _context.People.Add(model);

                    await _context.SaveChangesAsync(); // Guarda el registro temporalmente

                    var emailResult = await SendEmailRegister(model);

                    if (emailResult is BadRequestObjectResult || emailResult is ObjectResult { StatusCode: 500 })
                    {
                        transaction.Rollback();
                        return emailResult; // Retorna el error de envío de correo
                    }

                    await transaction.CommitAsync(); // Confirma la transacción

                    return Ok("Registro existoso, Te hemos enviado un código de activación a tu correo.");
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"SQL Error: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"System Error: {ex.Message}");
                }
            }
        }

        private async Task<IActionResult> SendEmailRegister(Person model)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Directory.GetCurrentDirectory())
                .UseMemoryCachingProvider()
                .Build();

            string templatePath = Path.Combine("Views", "Register", "RegisterConfirmation.cshtml");
            string htmlContent = await engine.CompileRenderAsync(templatePath, model);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LegacyTest", "sebasdazac@gmail.com"));
            message.To.Add(new MailboxAddress(model.Name, model.Email));
            message.Subject = "Registro exitoso";

            var builder = new BodyBuilder();

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/logo-legacytest.png");
            var logo = builder.LinkedResources.Add(logoPath);
            logo.ContentId = MimeUtils.GenerateMessageId();





            builder.HtmlBody = htmlContent.Replace("cid:logo", "cid:" + logo.ContentId);

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:EmailApplication"], _configuration["SmtpSettings:PasswordApplication"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al enviar el correo: {ex.Message}");
                }
            }

            return Ok("Correo enviado exitosamente.");
        }
    }
}
