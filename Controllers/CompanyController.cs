using Microsoft.AspNetCore.Mvc;

namespace LegacyTest.Controllers
{
    public class CompanyController : Controller
    {

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
    }
}
