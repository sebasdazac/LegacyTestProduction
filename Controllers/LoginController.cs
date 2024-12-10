using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LegacyTest.Tools;
using LegacyTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace LegacyTest.Controllers
{
    public class LoginController : Controller
    {
        private readonly LegacyDBContext _context;
        private readonly Crypto crypto = new();

        public LoginController(LegacyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Dashboard");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Close()
        {
            // Registrar salida de sesión actual
            var userId = User.FindFirst("IdPerson")?.Value;
            var session = await _context.Sessions
                .Where(s => s.PersonId == Convert.ToInt64(userId) && s.DateEnd == null)
                .OrderByDescending(s => s.DateStart)
                .FirstOrDefaultAsync();

            if (session != null)
            {
                session.DateEnd = DateTime.Now;
                _context.Update(session);
                await _context.SaveChangesAsync();
            }

            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Session(string email, string pswd)
        {
            try
            {
                var login = await _context.People.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper()
                                                    && x.Pswd == crypto.Encrypt(pswd));

                if (login == null)
                {
                    return Json(new { success = false, errorMessage = "Error de credenciales" });
                }

                var planCompany = await _context.PlanCompanies.FirstOrDefaultAsync(x => x.IdCompany == login.IdCompany && x.IsActive == true);

                if (planCompany == null)
                {
                    return Json(new { success = false, errorMessage = "No tiene un plan asociado" });
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, login.Name + " " + login.Surname));
                identity.AddClaim(new Claim(ClaimTypes.Email, login.Email.ToString()));
                identity.AddClaim(new Claim("IdPerson", login.Id.ToString()));
                identity.AddClaim(new Claim("IdRole", login.IdRole.ToString()));
                identity.AddClaim(new Claim("IdCompany", login.IdCompany.ToString()));
                identity.AddClaim(new Claim("IdPlanCompany", planCompany.Id.ToString()));
                identity.AddClaim(new Claim("IdPlan", planCompany.IdPlan.ToString()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(20),
                        IsPersistent = true
                    }
                );

                // Registrar inicio de sesión en la tabla Session
                var session = new Session
                {
                    PersonId = login.Id,
                    IdPlanCompany = planCompany.Id,
                    DateStart = DateTime.Now,
                    Stated = "Activo"
                };

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();

                return Json(new { success = true, data = login });
            }
            catch (FormatException ex)
            {
                return Json(new { success = false, errorMessage = $"Error al iniciar sesión: {ex.Message}" });
            }
            catch (SqlException ex)
            {
                return Json(new { success = false, errorMessage = $"Error al iniciar sesión: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"Error al iniciar sesión: {ex.Message}" });
            }
        }
    }
}
