using LegacyTest.Models;
using LegacyTest.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyTest.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly LegacyDBContext _context;
        private readonly Crypto crypto = new();


        public CompanyController(LegacyDBContext context, IConfiguration configuration)
        {

            this._context = context;
            this._configuration = configuration;
        }


        public IActionResult Account()
        {
            ViewBag.company = "active";
            ViewBag.menuCompany = "show";
            ViewBag.account = "active";
            return View();
        }
        public IActionResult Organization()
        {
            ViewBag.company = "active";
            ViewBag.menuCompany = "show";
            ViewBag.organization = "active";
            return View();
        }

        public IActionResult Plans()
        {
            ViewBag.company = "active";
            ViewBag.menuCompany = "show";
            ViewBag.plans = "active";
            return View();
        }




        public IActionResult GetOrganizationInfo()
        {
            try
            {
                int idCompany = Int32.Parse(SessionHelper.GetValue(User, "IdCompany"));
                var company = _context.Companies.FirstOrDefault(x => x.Id == idCompany);
                return Json(company);
            }

            catch (FormatException)
            {
                return Json(new { success = false, message = "Error en el formato de la consulta." });
            }
            catch (SystemException)
            {
                return Json(new { success = false, message = "Error de sistema. Por favor, intente nuevamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la solicitud. " + ex.Message });
            }

        }


        [HttpPost]
        public IActionResult UpdateOrganizationInfo(string businessName, string typeReg, string commercialReg)
        {
            try
            {
                int idCompany = Int32.Parse(SessionHelper.GetValue(User, "IdCompany"));
                var company = _context.Companies.FirstOrDefault(x => x.Id == idCompany);

                if (company == null)
                {
                    return Json(new { success = false, message = "Compañía no encontrada." });
                }

                company.BusinessName = businessName;
                company.TypeReg = typeReg;
                company.CommercialReg = commercialReg;

                _context.SaveChanges();

                return Json(new { success = true, message = "Información de la compañía actualizada con éxito." });
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = "Error en el formato de los datos." });
            }
            catch (SystemException)
            {
                return Json(new { success = false, message = "Error de sistema. Por favor, intente nuevamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la solicitud. " + ex.Message });
            }
        }



        [HttpPost]
        public IActionResult ActivatePlanFree(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "El token no puede estar vacío." });
                }

                var personExist = _context.People.FirstOrDefault(x => x.Token == token && x.State == "Inactivo");

                if (personExist != null)
                {
                    personExist.State = "Activo";
                    personExist.IdRole = 2;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Plan activado con éxito, debe REINICIAR SESIÓN para ver los cambios." });
                }
                else
                {
                    return Json(new { success = false, message = "Token no válido o la persona ya está activa." });
                }
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = "Error en el formato del token." });
            }
            catch (SystemException)
            {
                return Json(new { success = false, message = "Error de sistema. Por favor, intente nuevamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la solicitud. " + ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetPeople()
        {
            try
            {
                int idCompany = Int32.Parse(SessionHelper.GetValue(User, "IdCompany"));
                var people = _context.People
                    .Where(p => p.IdCompany == idCompany && p.State == "Activo")
                    .Select(p => new
                    {
                        name = p.Name,
                        surname = p.Surname,
                        email = p.Email,
                        state = p.State,
                        role = p.IdRoleNavigation.Role // Asegúrate de que RoleName es la propiedad correcta para el nombre del rol
                    })
                    .ToList();

                return Json(people);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener la lista de personas. " + ex.Message });
            }
        }
    }
}
