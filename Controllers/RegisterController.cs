using LegacyTest.Models;
using LegacyTest.Models.PersonAux;
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

        public IActionResult Index()
        {
            return View();
        }

        private static string GenerateCompanyFree(string nombreBase)
        {
            string fechaActual = DateTime.Now.ToString("yyyyMMdd_HHmm");
            return $"{nombreBase}_{fechaActual}";
        }


        [HttpPost]
        public async Task<IActionResult> RegisterPerson([FromBody] Person model)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
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
                            await transaction.RollbackAsync();
                            return emailResult; // Retorna el error de envío de correo
                        }

                        await transaction.CommitAsync(); // Confirma la transacción

                        return Ok("Registro exitoso, Te hemos enviado un código de activación a tu correo.");
                    }
                    catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, $"SQL Error: {sqlEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, $"System Error: {ex.Message}");
                    }
                }
            });
        }



        private async Task<IActionResult> SendEmailRegister(Person model)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Directory.GetCurrentDirectory())
                .UseMemoryCachingProvider()
                .Build();

            // Compilar la plantilla Razor
            string templatePath = Path.Combine("Templates", "RegisterConfirmation.cshtml");
            string htmlContent = await engine.CompileRenderAsync(templatePath, model);

            // Configuración del mensaje de correo
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LegacyTest", _configuration["SmtpSettings:EmailApplication"]));
            message.To.Add(new MailboxAddress(model.Name, model.Email));
            message.Subject = "Registro exitoso";

            var builder = new BodyBuilder();

            // Embeder el logo
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/logo-legacytest.png");
            if (System.IO.File.Exists(logoPath))
            {
                var logo = builder.LinkedResources.Add(logoPath);
                logo.ContentId = MimeUtils.GenerateMessageId();

                // Reemplazar el "cid" del logo en el HTML
                htmlContent = htmlContent.Replace("cid:logo", "cid:" + logo.ContentId);
            }
            else
            {
                return StatusCode(404, "El logo no se encuentra en la ruta especificada.");
            }

            // Embeder la imagen de registro
            var registerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/register.jpg");
            if (System.IO.File.Exists(registerImagePath))
            {
                var registerImage = builder.LinkedResources.Add(registerImagePath);
                registerImage.ContentId = MimeUtils.GenerateMessageId();

                // Reemplazar el "cid" de la imagen de registro en el HTML
                htmlContent = htmlContent.Replace("cid:register", "cid:" + registerImage.ContentId);
            }
            else
            {
                return StatusCode(404, "La imagen de registro no se encuentra en la ruta especificada.");
            }

            // Configuración del cuerpo del correo
            builder.HtmlBody = htmlContent;
            message.Body = builder.ToMessageBody();

            // Enviar el correo usando el cliente SMTP
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



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); 
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPasswordEmail([FromForm] string email)
        {
            try
            {
                var user = await _context.People.FirstOrDefaultAsync(p => p.Email.ToUpper() == email.ToUpper());

                if (user != null)
                {
                    user.ResetToken = GenerateToken();
                    user.ResetTokenExpiry = DateTime.Now.AddHours(1);
                    await _context.SaveChangesAsync();

                    await SendPasswordResetEmail(user);
                }
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, $"Error de base de datos al procesar la solicitud de restablecimiento de contraseña: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud de restablecimiento de contraseña: {ex.Message}");
            }

            return Ok("Si el correo está registrado, recibirás un enlace para restablecer tu contraseña.");
        }

        [HttpGet]
        public IActionResult ResetPasswordForm(string token)
        {
            try
            {
                var user = _context.People.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
                if (user == null)
                {
                    return BadRequest("Token inválido o expirado.");
                }
                return View(new ResetPasswordModel { Token = token });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, $"Error de base de datos al cargar el formulario de restablecimiento de contraseña: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cargar el formulario de restablecimiento de contraseña: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                var user = _context.People.FirstOrDefault(u => u.ResetToken == model.Token && u.ResetTokenExpiry > DateTime.Now);
                if (user == null)
                {
                    return BadRequest(new { message = "Token inválido o expirado." });
                }

                user.Pswd = crypto.Encrypt(model.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                _context.SaveChanges();

                return Ok(new { message = "Contraseña restablecida con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al restablecer la contraseña: {ex.Message}" });
            }
        }


        private async Task<IActionResult> SendPasswordResetEmail(Person user)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Directory.GetCurrentDirectory())
                .UseMemoryCachingProvider()
                .Build();

            string templatePath = Path.Combine("Templates", "ResetPassword.cshtml");
            string htmlContent;

            var resetUrl = Url.Action("ResetPasswordForm", "Register", new { token = user.ResetToken }, Request.Scheme);

            try
            {
                htmlContent = await engine.CompileRenderAsync(templatePath, new { Token = user.ResetToken, Name = user.Name, ResetUrl = resetUrl });
            }
            catch
            {
                return StatusCode(500, "Error interno al procesar la plantilla.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LegacyTest", _configuration["SmtpSettings:EmailApplication"]));
            message.To.Add(new MailboxAddress(user.Name, user.Email));
            message.Subject = "Restablecimiento de contraseña";

            var builder = new BodyBuilder();

            try
            {
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/logo-legacytest.png");
                var resetImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/register.jpg");

                if (System.IO.File.Exists(logoPath))
                {
                    var logo = builder.LinkedResources.Add(logoPath);
                    logo.ContentId = MimeUtils.GenerateMessageId();
                    htmlContent = htmlContent.Replace("cid:logo", "cid:" + logo.ContentId);
                }
                else
                {
                    return StatusCode(404, "Recurso de logo no disponible.");
                }

                if (System.IO.File.Exists(resetImagePath))
                {
                    var resetImage = builder.LinkedResources.Add(resetImagePath);
                    resetImage.ContentId = MimeUtils.GenerateMessageId();
                    htmlContent = htmlContent.Replace("cid:register", "cid:" + resetImage.ContentId);
                }
                else
                {
                    return StatusCode(404, "Recurso de imagen no disponible.");
                }

                builder.HtmlBody = htmlContent;
                message.Body = builder.ToMessageBody();
            }
            catch
            {
                return StatusCode(500, "Error al cargar los recursos del correo.");
            }

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:EmailApplication"], _configuration["SmtpSettings:PasswordApplication"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch
                {
                    return StatusCode(500, "Error al enviar el correo.");
                }
            }

            return Ok("Correo de restablecimiento enviado exitosamente.");
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
    }
}
