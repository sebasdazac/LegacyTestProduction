using LegacyTest.Models.Request;
using LegacyTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegacyTest.Tools;
using jsreport.AspNetCore;
using jsreport.Types;
using LegacyTest.Models.ReportAux;
using Microsoft.AspNetCore.Authorization;



namespace LegacyTest.Controllers
{
    public class ReportsController : Controller
    {

        private readonly LegacyDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IJsReportMVCService JsReportMVCService { get; }

        public ReportsController(LegacyDBContext context, IJsReportMVCService jsReportMVCService, IWebHostEnvironment hostingEnvironment)
        {
            JsReportMVCService = jsReportMVCService;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

     
        public async Task<IActionResult> Index()
        {
            ViewBag.reports = "active";
            ViewBag.reportCompany = "show";
            var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));

            var solvedForms = _context.CharacterizationByCompanies
                .Where(x => x.IdCompany == idCompany)
                .Select(x => x.IdForm)
                .Distinct()
                .ToList();

            var activesForms = await _context.PlanCompanies
                .Include(pc => pc.IdPlanNavigation.FormPlans)
                    .ThenInclude(fp => fp.IdFormNavigation)
                        .ThenInclude(f => f.ReportScales)
                .Where(pc => pc.IdCompany == idCompany)
                .SelectMany(pc => pc.IdPlanNavigation.FormPlans)
                .Where(fp => fp.IsActive == true && solvedForms.Contains(fp.IdFormNavigation.Id))
                .GroupBy(fp => fp.IdFormNavigation.Id) // Agrupa por IdForm
                .Select(group => new ReportRequest
                {
                    Id = group.Key, // Id del formulario
                    NameForm = group.First().IdFormNavigation.NameForm, // Nombre del formulario
                    DescriptionReport = group.First().IdFormNavigation.DescriptionReport, // Descripción del formulario
                    TitleScale = group.First().IdFormNavigation.ReportScales.FirstOrDefault().Title, // Título de la escala del primer ReportScale
                    ReportScaleList = group.SelectMany(fp => fp.IdFormNavigation.ReportScales).ToList() // Lista de ReportScales para el formulario
                })
                .ToListAsync();

            return View(activesForms);
        }



        //[MiddlewareFilter(typeof(JsReportPipeline))]
        //public async Task<IActionResult> GeneratePdf(long idForm)
        //{

        //    try
        //    {
        //        var idPerson = long.Parse(SessionHelper.GetValue(User, "IdPerson"));
        //        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
        //        var idPlanCompany = long.Parse(SessionHelper.GetValue(User, "IdPlanCompany"));

        //        FormReport form = await _context.Forms
        //                          .Where(x => x.Id == idForm)
        //                          .Select(x => new FormReport
        //                          {
        //                              Id = x.Id,
        //                              NameForm = x.NameForm,
        //                              DescriptionReport = x.DescriptionReport,
        //                              IsActive = x.IsActive
        //                          })
        //                          .FirstOrDefaultAsync();



        //        List<ReportScale> scales = await _context.ReportScales.Where(x => x.IdForm == idForm).ToListAsync();

        //        List<CharacterizationByCompany> criterionCharacterizations = await _context.CharacterizationByCompanies
        //                                               .Where(x => x.IdCompany == idCompany
        //                                                        && x.IdPlanCompany == idPlanCompany
        //                                                        && x.IdForm == idForm)
        //                                               .ToListAsync();


        //        List<long> characterizationIds = criterionCharacterizations.Select(cc => cc.IdCharacterization).ToList();

        //        List<EffectReport> effects = await _context.CharacterizationEffects
        //                                              .Select(x => new EffectReport
        //                                              {
        //                                                  Id = x.Id,
        //                                                  IdCharacterization = x.IdCharacterization,
        //                                                  EffectDescription = x.Effect ?? string.Empty,
        //                                                  EffectType = x.EffectType ?? string.Empty
        //                                              })
        //                                             .Where(x => characterizationIds.Contains(x.IdCharacterization))
        //                                             .ToListAsync();

        //        List<RecommendationReport> recomendations = await _context.CharacterizationRecomendations
        //                                                    .Select(x => new RecommendationReport
        //                                                    {
        //                                                        Id = x.Id,
        //                                                        IdCharacterization = x.IdCharacterization,
        //                                                        RecommendationDescription = x.Recomendation,
        //                                                        RecommendationType = x.RecomendationType
        //                                                    })
        //                                                    .Where(x => characterizationIds.Contains(x.IdCharacterization))
        //                                                    .ToListAsync();

        //        var combinedResults = from characterization in criterionCharacterizations
        //                              join effect in effects
        //                              on characterization.IdCharacterization equals effect.IdCharacterization into effectGroup
        //                              from eg in effectGroup.DefaultIfEmpty()  // Incluye todos los characterization, incluso si no hay efectos
        //                              join recommendation in recomendations
        //                              on characterization.IdCharacterization equals recommendation.IdCharacterization into recommendationGroup
        //                              from rg in recommendationGroup.DefaultIfEmpty()  // Incluye todas las recomendaciones, incluso si no hay recomendaciones
        //                              group new { eg, rg } by characterization into combinedGroup
        //                              select new CombinedResult
        //                              {
        //                                  IdCharacterization = combinedGroup.Key.IdCharacterization,
        //                                  Characterization = combinedGroup.Key.Characterization,
        //                                  Effects = combinedGroup.Where(x => x.eg != null).Select(x => x.eg).Distinct().ToList(),
        //                                  Recommendations = combinedGroup.Where(x => x.rg != null).Select(x => x.rg).Distinct().ToList()
        //                              };




        //        var resultList = combinedResults.ToList();

        //        ResponseReport responseReport = new ResponseReport
        //        {
        //            Form = form,
        //            Scales = scales,
        //            CombinedResults = resultList
        //        };

        //        HttpContext.JsReportFeature()
        //           .Recipe(Recipe.ChromePdf)
        //           .Configure(cfg =>
        //           {
        //               cfg.Template.Chrome = new Chrome
        //               {
        //                   MarginTop = "1cm",
        //                   MarginLeft = "1cm",
        //                   MarginBottom = "1cm",
        //                   MarginRight = "1cm",
        //                   WaitForJS = true
        //               };
        //           });

        //        var reportExport = new ReportExport
        //        {
        //            IdPerson = idPerson,
        //            IdPlanCompany = idPlanCompany,
        //            IdCompany = idCompany,
        //            IdForm = idForm,
        //            ExportDate = DateTime.Now
        //        };

        //        _context.ReportExports.Add(reportExport);
        //        await _context.SaveChangesAsync();


        //        return View("ReportPdf", responseReport);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}




        //public async Task<IActionResult> PartialReport(long idForm)
        //{

        //    try
        //    {
        //        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
        //        var idPlanCompany = long.Parse(SessionHelper.GetValue(User, "IdPlanCompany"));


        //        FormReport form = await _context.Forms
        //                          .Where(x => x.Id == idForm)
        //                          .Select(x => new FormReport
        //                          {
        //                              Id = x.Id,
        //                              NameForm = x.NameForm,
        //                              DescriptionReport = x.DescriptionReport,
        //                              IsActive = x.IsActive
        //                          })
        //                          .FirstOrDefaultAsync();



        //        List<ReportScale> scales = await _context.ReportScales.Where(x => x.IdForm == idForm).ToListAsync();

        //        List<CharacterizationByCompany> criterionCharacterizations = await _context.CharacterizationByCompanies
        //                                               .Where(x => x.IdCompany == idCompany
        //                                                        && x.IdPlanCompany == idPlanCompany
        //                                                        && x.IdForm == idForm)
        //                                               .ToListAsync();


        //        List<long> characterizationIds = criterionCharacterizations.Select(cc => cc.IdCharacterization).ToList();

        //        List<EffectReport> effects = await _context.CharacterizationEffects
        //                                              .Select(x => new EffectReport
        //                                              {
        //                                                  Id = x.Id,
        //                                                  IdCharacterization = x.IdCharacterization,
        //                                                  EffectDescription = x.Effect ?? string.Empty,
        //                                                  EffectType = x.EffectType ?? string.Empty
        //                                              })
        //                                             .Where(x => characterizationIds.Contains(x.IdCharacterization))
        //                                             .ToListAsync();

        //        List<RecommendationReport> recomendations = await _context.CharacterizationRecomendations
        //                                                    .Select(x => new RecommendationReport
        //                                                    {
        //                                                        Id = x.Id,
        //                                                        IdCharacterization = x.IdCharacterization,
        //                                                        RecommendationDescription = x.Recomendation,
        //                                                        RecommendationType = x.RecomendationType
        //                                                    })
        //                                                    .Where(x => characterizationIds.Contains(x.IdCharacterization))
        //                                                    .ToListAsync();

        //        var combinedResults = from characterization in criterionCharacterizations
        //                              join effect in effects
        //                              on characterization.IdCharacterization equals effect.IdCharacterization into effectGroup
        //                              from eg in effectGroup.DefaultIfEmpty()  // Incluye todos los characterization, incluso si no hay efectos
        //                              join recommendation in recomendations
        //                              on characterization.IdCharacterization equals recommendation.IdCharacterization into recommendationGroup
        //                              from rg in recommendationGroup.DefaultIfEmpty()  // Incluye todas las recomendaciones, incluso si no hay recomendaciones
        //                              group new { eg, rg } by characterization into combinedGroup
        //                              select new CombinedResult
        //                              {
        //                                  IdCharacterization = combinedGroup.Key.IdCharacterization,
        //                                  Characterization = combinedGroup.Key.Characterization,
        //                                  Effects = combinedGroup.Where(x => x.eg != null).Select(x => x.eg).Distinct().ToList(),
        //                                  Recommendations = combinedGroup.Where(x => x.rg != null).Select(x => x.rg).Distinct().ToList()
        //                              };




        //        var resultList = combinedResults.ToList();

        //        ResponseReport responseReport = new ResponseReport
        //        {
        //            Form = form,
        //            Scales = scales,
        //            CombinedResults = resultList
        //        };

        //        return PartialView("_PartialReport", responseReport);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

    }
}
