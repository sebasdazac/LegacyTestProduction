using Microsoft.AspNetCore.Mvc;

namespace LegacyTest.Controllers
{
    public class PricingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
