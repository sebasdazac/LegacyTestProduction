using LegacyTest.Models;
using LegacyTest.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LegacyTest.Controllers
{
   
    public class CharacterizationByCompanyController : Controller
    {

        private readonly LegacyDBContext _context;

        public CharacterizationByCompanyController(LegacyDBContext context)
        {
            _context = context;

        }

        private static string GetLabelCriterion1(string Criterion)
        {
            string label = "";

            if (Criterion.Contains("E"))
            {
                label = "EMPRESA";
            }
            if (Criterion.Contains("P"))
            {
                label = "PATRIMONIO";
            }

            if (Criterion.Contains("F"))
            {
                label = "FAMILIA";
            }
            return label;
        }

        public async Task<IActionResult> GetChart1(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }


                var data = await _context.CharacterizationByCompanies
                    .Include(x => x.IdCharacterizationNavigation)
                        .ThenInclude(x => x.CharacterizationEffects) // Asegura la inclusión de CharacterizationEffects
                    .Include(x => x.IdCharacterizationNavigation)
                        .ThenInclude(x => x.CharacterizationRecomendations) // Asegura la inclusión de CharacterizationRecomendations

                    .Where(x => x.IdForm == 1
                             && x.IdCompany == idCompany)

                    .Where(x => x.IdForm == 1)

                    .Select(x => new
                    {
                        label = GetLabelCriterion1(x.CriterionText1),
                        x = x.CriterionText2 + x.IdClasification1.Substring(0, 1).ToUpper(),
                        y = x.CriterionText1 + x.IdClasification2.Substring(0, 1).ToUpper(),
                        text = x.IdClasification1,
                        idCharacterization = x.IdCharacterization,
                        characterization = x.Characterization,
                        criterion1 = x.IdCriterio1,
                        criterion2 = x.IdCriterio2,
                        value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                        value2 = (int)Math.Round(((double)(x.AverageCriterion2 ?? 0)) / 5 * 100),
                        effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                        recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                    })
                    .ToListAsync();

                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart2(string param = null)
        {
            try
            {

                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                   .Include(x => x.IdCharacterizationNavigation)
                       .ThenInclude(x => x.CharacterizationEffects)
                   .Include(x => x.IdCharacterizationNavigation)
                       .ThenInclude(x => x.CharacterizationRecomendations)
                   .Where(x => x.IdForm == 2
                            && x.IdCompany == idCompany)
                   .Select(x => new
                   {
                       label = x.Characterization,
                       idCharacterization = x.IdCharacterization,
                       characterization = x.Characterization,
                       criterion1 = x.IdClasification1,
                       criterion2 = x.IdClasification2,
                       value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                       value2 = (int)Math.Round(((double)(x.AverageCriterion2 ?? 0)) / 5 * 100),
                       effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(),
                       recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList()
                   })
                   .FirstOrDefaultAsync();


                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart3(string param = null)
        {
            try
            {

                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 3
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             value2 = (int)Math.Round(((double)(x.AverageCriterion2 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(),
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList()
                                         })
                                         .FirstOrDefaultAsync();

                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }


            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetChart4(string param = null)
        {
            try
            {


                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                         .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                         .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 4 && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.CriterionText1 ?? "",
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             value2 = (int)Math.Round(((double)(x.AverageCriterion2 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1 ?? "",
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization ?? "",
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects
                                                 .Select(e => new { e.Effect, e.EffectType })
                                                 .ToList(),
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations
                                                 .Select(r => new { r.Recomendation, r.RecomendationType })
                                                 .ToList()
                                         })
                                         .ToListAsync();

                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetChart5(string param = null)
        {
            try
            {

                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                        .Include(x => x.IdCharacterizationNavigation)
                                        .ThenInclude(x => x.CharacterizationEffects)
                                        .Include(x => x.IdCharacterizationNavigation)
                                        .ThenInclude(x => x.CharacterizationRecomendations)
                                        .Where(x => x.IdForm == 5
                                                    && x.IdCompany == idCompany)
                                        .Select(x => new
                                        {
                                            label = x.CriterionText1,
                                            value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                            idCharacterization = x.IdCharacterization,
                                            text = x.IdClasification1,
                                            characterization = x.Characterization,
                                            effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                            recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                        })
                                        .ToListAsync();


                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetChart6(string param = null)
        {
            try
            {

                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                        .Include(x => x.IdCharacterizationNavigation)
                                        .ThenInclude(x => x.CharacterizationEffects)
                                        .Include(x => x.IdCharacterizationNavigation)
                                        .ThenInclude(x => x.CharacterizationRecomendations)
                                        .Where(x => x.IdForm == 6
                                                    && x.IdCompany == idCompany)
                                        .Select(x => new
                                        {
                                            label = x.CriterionText1,
                                            value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                            text = x.IdClasification1,
                                            idCharacterization = x.IdCharacterization,
                                            characterization = x.Characterization,
                                            effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                            recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones

                                        })
                                            .ToListAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 6)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                .Include(x => x.IdQuestionNavigation)
                                                .ThenInclude(x => x.IdDimensionNavigation)
                                                .Where(x => listQuestions.Contains(x.IdQuestion))
                                                .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                .Select(g => new
                                                {
                                                    label = g.Key,
                                                    averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                })
                                                .ToListAsync();


                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<IActionResult> GetChart7(string param = null)
        {
            try
            {

                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                         .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                         .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 7
                                                && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = "",
                                             value1 = (double)Math.Round((double)x.AverageCriterion1 / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 7)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                .Include(x => x.IdQuestionNavigation)
                                                .ThenInclude(x => x.IdDimensionNavigation)
                                                .Where(x => listQuestions.Contains(x.IdQuestion))
                                                .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                .Select(g => new
                                                {
                                                    label = g.Key,
                                                    averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                })
                                                .ToListAsync();


                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetChart8(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 8
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             value2 = (int)Math.Round(((double)(x.AverageCriterion2 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 8)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                .Include(x => x.IdQuestionNavigation)
                                                .ThenInclude(x => x.IdDimensionNavigation)
                                                .Where(x => listQuestions.Contains(x.IdQuestion))
                                                .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                .Select(g => new
                                                {
                                                    label = g.Key,
                                                    averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                })
                                                .ToListAsync();


                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart9(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 9
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 9)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart10(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 10
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 10)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                         .Select(g => new
                                                         {
                                                             label = g.Key,
                                                             averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                         })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart11(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }


                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 11
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 11)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }



        public async Task<IActionResult> GetChart12(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                     .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                            .Where(x => x.IdForm == 12
                                                     && x.IdCompany == idCompany)
                                            .Select(x => new
                                            {
                                                label = x.CriterionText1,
                                                value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                                text = x.IdClasification1,
                                                idCharacterization = x.IdCharacterization,
                                                characterization = x.Characterization,
                                                effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                                recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones

                                            })
                                              .ToListAsync();
                return Json(data);

            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }

        }


        public async Task<IActionResult> GetChart13(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 13
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 13)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<IActionResult> GetChart14(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 14
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 14)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }




        public async Task<IActionResult> GetChart15(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                     .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                            .Where(x => x.IdForm == 15
                                                     && x.IdCompany == idCompany)
                                            .Select(x => new
                                            {
                                                label = x.CriterionText1,
                                                value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                                text = x.IdClasification1,
                                                idCharacterization = x.IdCharacterization,
                                                characterization = x.Characterization,
                                                effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                                recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones

                                            })
                                              .ToListAsync();
                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }




        public async Task<IActionResult> GetChart16(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 16
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 16)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<IActionResult> GetChart17(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                         .Where(x => x.IdForm == 17
                                                  && x.IdCompany == idCompany)
                                         .Select(x => new
                                         {
                                             label = x.Characterization,
                                             value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                             text = x.IdClasification1,
                                             idCharacterization = x.IdCharacterization,
                                             characterization = x.Characterization,
                                             effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                             recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones
                                         })
                                         .FirstOrDefaultAsync();

                var listQuestions = await _context.Questions
                                            .Include(x => x.IdCriterionNavigation)
                                            .ThenInclude(x => x.IdFormNavigation)
                                            .Where(x => x.IdCriterionNavigation.IdFormNavigation.Id == 17)
                                            .Select(x => x.Id)
                                            .ToListAsync();

                var dimensions = await _context.AnswerCompanies
                                                        .Include(x => x.IdQuestionNavigation)
                                                        .ThenInclude(x => x.IdDimensionNavigation)
                                                        .Where(x => listQuestions.Contains(x.IdQuestion))
                                                        .GroupBy(x => x.IdQuestionNavigation.IdDimensionNavigation.DimensionText)
                                                        .Select(g => new
                                                        {
                                                            label = g.Key,
                                                            averageValue = (int)Math.Round(g.Average(x => Math.Round((double)x.AverageQuestion / 5 * 100)))
                                                        })
                                                        .ToListAsync();



                return Json(new { data, dimensions });
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetChart18(string param = null)
        {
            try
            {
                long idCompany;

                if (!string.IsNullOrEmpty(param))
                {
                    idCompany = long.Parse(param);
                }
                else
                {
                    idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));
                }

                var data = await _context.CharacterizationByCompanies
                     .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationEffects)
                                         .Include(x => x.IdCharacterizationNavigation)
                                           .ThenInclude(x => x.CharacterizationRecomendations)
                                            .Where(x => x.IdForm == 18
                                                     && x.IdCompany == idCompany)
                                            .Select(x => new
                                            {
                                                label = x.CriterionText1,
                                                value1 = (int)Math.Round(((double)(x.AverageCriterion1 ?? 0)) / 5 * 100),
                                                text = x.IdClasification1,
                                                idCharacterization = x.IdCharacterization,
                                                characterization = x.Characterization,
                                                effects = x.IdCharacterizationNavigation.CharacterizationEffects.Select(e => new { e.Effect, e.EffectType }).ToList(), // Selecciona los campos necesarios de los efectos
                                                recommendations = x.IdCharacterizationNavigation.CharacterizationRecomendations.Select(r => new { r.Recomendation, r.RecomendationType }).ToList() // Selecciona los campos necesarios de las recomendaciones

                                            })
                                              .ToListAsync();
                return Json(data);
            }
            catch (FormatException e)
            {
                throw;
            }

            catch (SqlException e)
            {
                throw;
            }

            catch (Exception e)
            {
                throw;
            }
        }

    }
}
