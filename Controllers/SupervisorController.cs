using LegacyTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegacyTest.Controllers
{
    public class SupervisorController : Controller
    {

        private readonly LegacyDBContext _context;

        public SupervisorController(LegacyDBContext context)
        { 
        
            _context = context;

        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetExportedReports()
        {
            var exportedReports = await (from re in _context.ReportExports
                                         join p in _context.People on re.IdPerson equals p.Id
                                         join c in _context.Companies on re.IdCompany equals c.Id
                                         join f in _context.Forms on re.IdForm equals f.Id
                                         where re.ExportDate != null
                                         select new
                                         {
                                             CompanyName = c.BusinessName,
                                             UserName = p.Name + " " + p.Surname,
                                             UserEmail = p.Email,
                                             ExportedForm = f.NameForm,
                                             ExportDate = re.ExportDate
                                         })
                                         .ToListAsync();

            return Json(exportedReports);
        }


        [HttpGet]
        public async Task<IActionResult> GetCompanyTransactions()
        {
            var companyTransactions = await (from tc in _context.TransactionCompanies
                                             join c in _context.Companies on tc.IdCompany equals c.Id
                                             select new
                                             {
                                                 CompanyName = c.BusinessName,
                                                 TransactionDate = tc.DateTransaction,
                                                 Amount = tc.Price,
                                                 Currency = tc.Currency,
                                                 PaymentMethod = tc.PaymentForm,
                                                 TransactionState = tc.StateTransaction,
                                                 ReferenceNumber = tc.NumberReference
                                             })
                                             .ToListAsync();

            return Json(companyTransactions);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoginSessions()
        {
            var loginSessions = await (from s in _context.Sessions
                                       join p in _context.People on s.PersonId equals p.Id
                                       join c in _context.Companies on p.IdCompany equals c.Id
                                       select new
                                       {
                                           companyName = c.BusinessName,
                                           userName = p.Name + " " + p.Surname,
                                           loginDate = s.DateStart,
                                           logoutDate = s.DateEnd,
                                           sessionState = s.Stated
                                       })
                                       .ToListAsync();

            return Json(loginSessions);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyPlans()
        {
            var companyPlans = await (from pc in _context.PlanCompanies
                                      join c in _context.Companies on pc.IdCompany equals c.Id
                                      join p in _context.Plans on pc.IdPlan equals p.Id
                                      select new
                                      {
                                          companyName = c.BusinessName,
                                          planName = p.NamePlan,
                                          planStatus = pc.IsActive,
                                          validityStart = pc.DateInitial,
                                          validityEnd = pc.DateEnd
                                      })
                                      .ToListAsync();

            return Json(companyPlans);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByCompany()
        {
            var usersByCompany = await (from p in _context.People
                                        join c in _context.Companies on p.IdCompany equals c.Id
                                        select new
                                        {
                                            companyName = c.BusinessName,
                                            userName = p.Name + " " + p.Surname,
                                            email = p.Email,
                                            isActive = p.State == "Activo" ? "Activo" : "Inactivo",
                                            registrationDate = p.ResetTokenExpiry // Cambia este campo si tienes un campo específico para la fecha de registro
                                        })
                                        .ToListAsync();

            return Json(usersByCompany);
        }


        [HttpGet]
        public async Task<IActionResult> GetFormCompletionByCompany()
        {
            var formCompletionByCompany = await (from ac in _context.AnswerCompanies
                                                 join c in _context.Companies on ac.IdCompany equals c.Id
                                                 join p in _context.PlanCompanies on ac.IdPlanCompany equals p.Id
                                                 join pl in _context.Plans on p.IdPlan equals pl.Id
                                                 join vfc in _context.ViewFormCriteria on ac.IdCriterion equals vfc.Id
                                                 join f in _context.Forms on vfc.IdForm equals f.Id
                                                 group f by new { c.BusinessName, pl.NamePlan, f.NameForm } into g
                                                 select new
                                                 {
                                                     companyName = g.Key.BusinessName,
                                                     planName = g.Key.NamePlan,
                                                     formName = g.Key.NameForm,
                                                     formCount = g.Select(x => x.NameForm).Distinct().Count(),
                                                     totalEntries = g.Count()
                                                 })
                                                 .ToListAsync();

            return Json(formCompletionByCompany);
        }



    }
}

