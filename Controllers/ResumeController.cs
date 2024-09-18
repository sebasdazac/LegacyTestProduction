using Microsoft.AspNetCore.Mvc;

namespace LegacyTest.Controllers
{
  public class ResumeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
