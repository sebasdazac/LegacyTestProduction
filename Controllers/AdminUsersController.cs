using Microsoft.AspNetCore.Mvc;

namespace LegacyTest.Controllers
{
    public class AdminUsersController : Controller
    {
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult Groups()
        {
            return View();
        }

        public IActionResult Companies()
        {
            return View();
        }
    }
}
