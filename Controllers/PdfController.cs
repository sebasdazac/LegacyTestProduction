using LegacyTest.Models.Request;
using LegacyTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LegacyTest.Tools;
using jsreport.AspNetCore;
using jsreport.Types;
using LegacyTest.Models.ReportAux;
using PdfSharp.Drawing;



namespace LegacyTest.Controllers
{
    public class PdfController : Controller
    {

        private readonly LegacyDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly PdfGeneratorService _pdfGeneratorService;
        public IJsReportMVCService JsReportMVCService { get; }

        public PdfController(LegacyDBContext context, IJsReportMVCService jsReportMVCService, IWebHostEnvironment hostingEnvironment, PdfGeneratorService pdfGeneratorService)
        {
            JsReportMVCService = jsReportMVCService;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _pdfGeneratorService = pdfGeneratorService;
        }


      [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> GenerateJReport(long idForm )
        {

            try
            {

                //var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                //var idPlanCompany = long.Parse(SessionHelper.GetValue(User, "IdPlanCompany"));

                var idCompany = 1;
                var idPlanCompany = 4;



                FormReport form = await _context.Forms
                                  .Where(x => x.Id == idForm)
                                  .Select(x => new FormReport
                                  {
                                      Id = x.Id,
                                      NameForm = x.NameForm,
                                      DescriptionReport = x.DescriptionReport,
                                      IsActive = x.IsActive
                                  })
                                  .FirstOrDefaultAsync();



                List<ReportScale> scales = await _context.ReportScales.Where(x => x.IdForm == idForm).ToListAsync();

                List<CharacterizationByCompany> criterionCharacterizations = await _context.CharacterizationByCompanies
                                                       .Where(x => x.IdCompany == idCompany
                                                                && x.IdPlanCompany == idPlanCompany
                                                                && x.IdForm == idForm)
                                                       .ToListAsync();


                List<long> characterizationIds = criterionCharacterizations.Select(cc => cc.IdCharacterization).ToList();

                List<EffectReport> effects = await _context.CharacterizationEffects
                                                      .Select(x => new EffectReport
                                                     {
                                                         Id = x.Id,
                                                         IdCharacterization = x.IdCharacterization,                                                         
                                                         EffectDescription = x.Effect ?? string.Empty,
                                                         EffectType = x.EffectType ?? string.Empty
                                                     })
                                                     .Where(x => characterizationIds.Contains(x.IdCharacterization))
                                                     .ToListAsync();

                List<RecommendationReport> recomendations = await _context.CharacterizationRecomendations                                                       
                                                            .Select(x => new RecommendationReport
                                                            {
                                                                Id = x.Id,                                                               
                                                                IdCharacterization = x.IdCharacterization,
                                                                RecommendationDescription = x.Recomendation,
                                                                RecommendationType = x.RecomendationType
                                                            })
                                                            .Where(x => characterizationIds.Contains(x.IdCharacterization))
                                                            .ToListAsync();

                var combinedResults = from characterization in criterionCharacterizations
                                      join effect in effects
                                      on characterization.IdCharacterization equals effect.IdCharacterization into effectGroup
                                      from eg in effectGroup.DefaultIfEmpty()  // Incluye todos los characterization, incluso si no hay efectos
                                      join recommendation in recomendations
                                      on characterization.IdCharacterization equals recommendation.IdCharacterization into recommendationGroup
                                      from rg in recommendationGroup.DefaultIfEmpty()  // Incluye todas las recomendaciones, incluso si no hay recomendaciones
                                      group new { eg, rg } by characterization into combinedGroup
                                      select new CombinedResult
                                      {
                                          IdCharacterization = combinedGroup.Key.IdCharacterization,
                                          Characterization = combinedGroup.Key.Characterization,
                                          Effects = combinedGroup.Where(x => x.eg != null).Select(x => x.eg).Distinct().ToList(),
                                          Recommendations = combinedGroup.Where(x => x.rg != null).Select(x => x.rg).Distinct().ToList()
                                      };




                var resultList = combinedResults.ToList();

                ResponseReport responseReport = new ResponseReport
                {
                    IdCompany = idCompany,
                    Form = form,
                    Scales = scales,                    
                    CombinedResults = resultList                    
                };

                HttpContext.JsReportFeature()
                   .Recipe(Recipe.ChromePdf)
                   .Configure(cfg =>
                   {
                       cfg.Template.Chrome = new Chrome
                       {
                           MarginTop = "1cm",
                           MarginLeft = "1cm",
                           MarginBottom = "1cm",
                           MarginRight = "1cm",
                          WaitForJS = true
                       };
                   });

                return View("PdfReport", responseReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        public async Task<IActionResult> Pdf1(long idForm,long idCompany,long idPlanCompany)
        {
            try
            {

                FormReport form = await _context.Forms
                                  .Where(x => x.Id == idForm)
                                  .Select(x => new FormReport
                                  {
                                      Id = x.Id,
                                      NameForm = x.NameForm,
                                      DescriptionReport = x.DescriptionReport,
                                      IsActive = x.IsActive
                                  })
                                  .FirstOrDefaultAsync();



                List<ReportScale> scales = await _context.ReportScales.Where(x => x.IdForm == idForm).ToListAsync();

                List<CharacterizationByCompany> criterionCharacterizations = await _context.CharacterizationByCompanies
                                                       .Where(x => x.IdCompany == idCompany
                                                                && x.IdPlanCompany == idPlanCompany
                                                                && x.IdForm == idForm)
                                                       .ToListAsync();


                List<long> characterizationIds = criterionCharacterizations.Select(cc => cc.IdCharacterization).ToList();

                List<EffectReport> effects = await _context.CharacterizationEffects
                                                      .Select(x => new EffectReport
                                                      {
                                                          Id = x.Id,
                                                          IdCharacterization = x.IdCharacterization,
                                                          EffectDescription = x.Effect ?? string.Empty,
                                                          EffectType = x.EffectType ?? string.Empty
                                                      })
                                                     .Where(x => characterizationIds.Contains(x.IdCharacterization))
                                                     .ToListAsync();

                List<RecommendationReport> recomendations = await _context.CharacterizationRecomendations
                                                            .Select(x => new RecommendationReport
                                                            {
                                                                Id = x.Id,
                                                                IdCharacterization = x.IdCharacterization,
                                                                RecommendationDescription = x.Recomendation,
                                                                RecommendationType = x.RecomendationType
                                                            })
                                                            .Where(x => characterizationIds.Contains(x.IdCharacterization))
                                                            .ToListAsync();

                var combinedResults = from characterization in criterionCharacterizations
                                      join effect in effects
                                      on characterization.IdCharacterization equals effect.IdCharacterization into effectGroup
                                      from eg in effectGroup.DefaultIfEmpty()  // Incluye todos los characterization, incluso si no hay efectos
                                      join recommendation in recomendations
                                      on characterization.IdCharacterization equals recommendation.IdCharacterization into recommendationGroup
                                      from rg in recommendationGroup.DefaultIfEmpty()  // Incluye todas las recomendaciones, incluso si no hay recomendaciones
                                      group new { eg, rg } by characterization into combinedGroup
                                      select new CombinedResult
                                      {
                                          IdCharacterization = combinedGroup.Key.IdCharacterization,
                                          Characterization = combinedGroup.Key.Characterization,
                                          Effects = combinedGroup.Where(x => x.eg != null).Select(x => x.eg).Distinct().ToList(),
                                          Recommendations = combinedGroup.Where(x => x.rg != null).Select(x => x.rg).Distinct().ToList()
                                      };




                var resultList = combinedResults.ToList();

                ResponseReport responseReport = new ResponseReport
                {
                    IdCompany = idCompany,
                    Form = form,
                    Scales = scales,
                    CombinedResults = resultList
                };

                return View(responseReport);
             
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }




        [HttpGet]
        public async Task<IActionResult> GeneratePdf()
        {

            long idForm = 1;
            long idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
            long idPlanCompany = long.Parse(SessionHelper.GetValue(User, "idPlanCompany"));


            var url = Url.Action("Pdf1", "Pdf", new { idForm = idForm, idCompany = idCompany, idPlanCompany = idPlanCompany }, Request.Scheme);

            // Genera el PDF
            var pdfBytes = await _pdfGeneratorService.GeneratePdfAsync(url);

            // Retorna el PDF como un archivo
            return File(pdfBytes, "application/pdf", "Reporte.pdf");

        }





    }
}
