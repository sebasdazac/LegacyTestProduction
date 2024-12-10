
using LegacyTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LegacyTest.Controllers
{
  
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly LegacyDBContext _context;

        public HomeController(LegacyDBContext context)
        {
            _context = context;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> DynamicPlan()
        {
            var ListPlans = await _context.Plans
                       .Where(x => x.IsActive == true)
                       .OrderBy(x=> x.Sort)
                       .ToListAsync();
                       
            if (ListPlans == null)
            {
                return NotFound();
            }
            return PartialView("_DynamicPlan", ListPlans);
        }
    }
}