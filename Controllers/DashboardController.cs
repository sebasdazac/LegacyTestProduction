using LegacyTest.Models;
using LegacyTest.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LegacyTest.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly LegacyDBContext _context;

        public DashboardController(LegacyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index() {
            ViewBag.dashboard = "active";
            ViewBag.main = "show";
            return View();
        }
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                var data = await _context.Forms
                         .Select(x => new { x.Id,x.NameForm })
                         .ToListAsync();


                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }

            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }
    }
}
