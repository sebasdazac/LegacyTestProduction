using Microsoft.AspNetCore.Mvc;

namespace LegacyTest.Controllers
{
    public class AdminFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
