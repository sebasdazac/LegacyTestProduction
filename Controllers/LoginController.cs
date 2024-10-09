using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using LegacyTest.Tools;
using Microsoft.EntityFrameworkCore;
using LegacyTest.Models;
using System;

namespace LegacyTest.Controllers
{
    public class LoginController : Controller
    {

        private readonly LegacyDBContext _context;
        private readonly Crypto crypto = new();


        public LoginController(LegacyDBContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Dashboard");
            }
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> Close()
        {
            await HttpContext.SignOutAsync();
            //return View("~/Views/Home/Index.cshtml");
            return Ok();
        }

     
        [HttpPost]
        public async Task<IActionResult> Session(string email, string pswd)
        {
            try
            {               
                
                var login = await _context.People.SingleAsync(x => x.Email.ToUpper() == email.ToUpper()
                                                && x.Pswd == crypto.Encrypt(pswd));

                var planCompany = await _context.PlanCompanies.SingleAsync(x => x.IdCompany == login.IdCompany && x.IsActive == true);                                                  
                

                if (login != null  && planCompany != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.Name, login.Name +" " +login.Surname));                  
                        identity.AddClaim(new Claim(ClaimTypes.Email, login.Email.ToString()));
                        identity.AddClaim(new Claim("IdPerson", login.Id.ToString()));
                        identity.AddClaim(new Claim("IdCompany", login.IdCompany.ToString()));
                        identity.AddClaim(new Claim("IdPlanCompany", planCompany.Id.ToString()));
                        identity.AddClaim(new Claim("IdPlan", planCompany.IdPlan.ToString()));


                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties 
                        { 
                            ExpiresUtc = DateTime.Now.AddDays(1), 
                            IsPersistent = true 
                        }
                    );

                    return Json(new { success = true, data = login });
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Error de credenciales o no tiene un plan asociado" });
                }
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
