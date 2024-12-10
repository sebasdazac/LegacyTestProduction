using LegacyTest.Models;
using LegacyTest.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;
using LegacyTest.Tools;
using System.ComponentModel.Design;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LegacyTest.Controllers
{
    [Authorize]
    public class QuestionaryController : Controller
    {

        private readonly LegacyDBContext _context;

        public QuestionaryController(LegacyDBContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index(int idCompany)
        {
            try
            {
                ViewBag.questionary = "active";
                ViewBag.planform = "show";

                var formIds = await _context.PlanCompanies
                                            .Where(x => x.IdCompany == idCompany)
                                            .SelectMany(x => x.IdPlanNavigation.FormPlans)
                                            .Select(x => x.IdForm)
                                            .ToListAsync();


                var forms = await _context.Forms
                                           .Where(f => formIds.Contains(f.Id))
                                           .ToListAsync();

                return View(forms);
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public IActionResult FillForm()
        {
            return View();
        }


        public async Task<IActionResult> QuestionaryByCompany(int idForm)
        {
            try
            {
                var idPerson = long.Parse(SessionHelper.GetValue(User, "idPerson"));
                var idCompany = long.Parse(SessionHelper.GetValue(User, "idCompany"));

                var idPlanCompany = await _context.PlanCompanies
                                            .Where(x => x.IsActive == true && x.IdCompany == idCompany)
                                            .Select(x => x.Id)
                                            .SingleOrDefaultAsync();

                var listForms = await _context.Forms
                            .Where(x => x.Id == idForm)
                            .Include(c => c.Criteria)
                            .ThenInclude(f => f.Questions)
                                .ThenInclude(q => q.Answers)
                            .FirstOrDefaultAsync();

                if (listForms == null)
                {
                    return NotFound();
                }

                var savedAnswers = await _context.AnswerPeople
                                   .Where(ap => ap.IdPerson == idPerson &&
                                                ap.IdCompany == idCompany &&
                                                ap.IdPlanCompany == idPlanCompany)
                                   .ToListAsync();

                savedAnswers = savedAnswers.Where(ap => listForms.Criteria.Any(x => x.Id == ap.IdCriterio))
                                           .ToList();

                var viewModel = new DynamicQuestionary
                {
                    Form = listForms,
                    SavedAnswers = savedAnswers
                };

                return View("QuestionaryByCompany", viewModel);
            }
            catch (FormatException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> LoadSavedAnswers(int formId)
        {
            try
            {
                var idPerson = long.Parse(SessionHelper.GetValue(User, "IdPerson"));
                var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));


                var savedAnswers = await _context.ViewQuestionAnswers
                                                 .Where(ap => ap.IdPerson == idPerson &&
                                                              ap.IdCompany == idCompany &&
                                                              ap.IdForm == formId)
                                                 .ToListAsync();

                return Json(savedAnswers);
            }
            catch (FormatException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }





        //public async Task<IActionResult> ProcessForm([FromBody] List<FormData> formDataList)
        //{
        //    try
        //    {
        //        var idPerson = long.Parse(SessionHelper.GetValue(User, "IdPerson"));
        //        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));

        //        // Obtenemos el ID del plan de la compañía
        //        var idPlanCompany = await _context.PlanCompanies
        //            .Where(x => x.IdCompany == idCompany && x.IsActive == true)
        //            .Select(x => x.Id)
        //            .FirstOrDefaultAsync();

        //        if (formDataList == null || formDataList.Count == 0)
        //        {
        //            return BadRequest("Debes contestar todas las preguntas");
        //        }

        //        List<string> errors = new List<string>();

        //        foreach (var formData in formDataList)
        //        {
        //            var valueAnswer = await _context.Answers
        //                .Where(x => x.Id == formData.IdAnswer)
        //                .Select(x => x.Value)
        //                .FirstOrDefaultAsync();

        //            var answer = _context.AnswerPeople.SingleOrDefault(x => x.IdPerson == idPerson
        //                                                                && x.IdCompany == idCompany
        //                                                                && x.IdPlanCompany == idPlanCompany
        //                                                                && x.IdQuestion == formData.IdQuestion
        //                                                                && x.IdCriterio == formData.IdCriterio);

        //            if (answer != null)
        //            {
        //                answer.IdAnswer = formData.IdAnswer;
        //                answer.Value = valueAnswer;
        //            }
        //            else
        //            {
        //                var answerPerson = new AnswerPerson
        //                {
        //                    IdQuestion = formData.IdQuestion,
        //                    IdAnswer = formData.IdAnswer,
        //                    IdCompany = idCompany,
        //                    IdPlanCompany = idPlanCompany,
        //                    IdPerson = idPerson,
        //                    IdCriterio = formData.IdCriterio,
        //                    Value = valueAnswer,
        //                    Date = DateTime.Now.Date
        //                };

        //                // Añadimos la respuesta del usuario a la base de datos
        //                _context.AnswerPeople.Add(answerPerson);
        //            }
        //        }

        //        // Guardamos los cambios en la base de datos
        //        await _context.SaveChangesAsync();

        //        // Actualizamos o creamos las respuestas de la compañía
        //        await UpdateOrCreateAnswerCompany(idCompany, idPlanCompany);

        //        await UpdateOrCreateAnswerCriterionCompany(idCompany, idPlanCompany);

        //        return Ok("Formulario procesado correctamente");
        //    }
        //    catch (FormatException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}


        //private async Task UpdateOrCreateAnswerCompany(long idCompany, long idPlanCompany)
        //{
        //    try
        //    {
        //        // Agrupamos las respuestas de las personas por pregunta y criterio y calculamos el promedio
        //        var answerPersonGroups = await _context.AnswerPeople
        //            .Where(x => x.IdCompany == idCompany && x.IdPlanCompany == idPlanCompany)
        //            .GroupBy(x => new { x.IdQuestion, x.IdCriterio })
        //            .Select(g => new
        //            {
        //                IdQuestion = g.Key.IdQuestion,
        //                IdCriterio = g.Key.IdCriterio,
        //                AverageValue = g.Average(x => Convert.ToDecimal(x.Value)) // Suponemos que el valor es numérico
        //            })
        //            .ToListAsync();

        //        foreach (var group in answerPersonGroups)
        //        {
        //            // Buscamos si ya existe una respuesta para la misma pregunta y criterio
        //            var existingAnswerCompany = _context.AnswerCompanies
        //                .SingleOrDefault(x => x.IdQuestion == group.IdQuestion
        //                                       && x.IdCriterion == group.IdCriterio
        //                                       && x.IdCompany == idCompany
        //                                       && x.IdPlanCompany == idPlanCompany);

        //            if (existingAnswerCompany != null)
        //            {
        //                // Actualizamos la entrada existente con el nuevo promedio
        //                existingAnswerCompany.AverageQuestion = Convert.ToDouble(group.AverageValue);
        //            }
        //            else
        //            {
        //                // Creamos una nueva entrada con el promedio
        //                var newAnswerCompany = new AnswerCompany
        //                {
        //                    IdQuestion = group.IdQuestion,
        //                    IdCriterion = group.IdCriterio,
        //                    IdCompany = idCompany,
        //                    IdPlanCompany = idPlanCompany,
        //                    AverageQuestion = Convert.ToDouble(group.AverageValue),
        //                    Date = DateTime.Now // Suponemos que deseas establecer la fecha actual
        //                };

        //                _context.AnswerCompanies.Add(newAnswerCompany);
        //            }
        //        }

        //        // Guardamos los cambios en la base de datos
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al actualizar o crear respuestas de la compañía", ex);
        //    }
        //}




        //private async Task UpdateOrCreateAnswerCriterionCompany(long idCompany, long idPlanCompany)
        //{
        //    try
        //    {
        //        // Agrupamos las respuestas de las personas por criterio y calculamos el promedio
        //        var answerCriterionGroups = await _context.AnswerCompanies
        //            .Where(x => x.IdCompany == idCompany && x.IdPlanCompany == idPlanCompany)
        //            .GroupBy(x => x.IdCriterion)
        //            .Select(g => new
        //            {
        //                IdCriterio = g.Key,
        //                AverageValue = g.Average(x => x.AverageQuestion)
        //            })
        //            .ToListAsync();

        //        foreach (var group in answerCriterionGroups)
        //        {
        //            // Buscamos si ya existe una respuesta para el mismo criterio
        //            var existingAnswerCriterionCompany = _context.AnswerCriterionCompanies
        //                .SingleOrDefault(x => x.IdCriterion == group.IdCriterio
        //                                       && x.IdCompany == idCompany
        //                                       && x.IdPlanCompany == idPlanCompany);

        //            if (existingAnswerCriterionCompany != null)
        //            {
        //                // Actualizamos la entrada existente con el nuevo promedio
        //                existingAnswerCriterionCompany.AverageCriterion = group.AverageValue;
        //            }
        //            else
        //            {
        //                // Creamos una nueva entrada con el promedio
        //                var newAnswerCriterionCompany = new AnswerCriterionCompany
        //                {
        //                    IdCriterion = group.IdCriterio,
        //                    IdCompany = idCompany,
        //                    IdPlanCompany = idPlanCompany,
        //                    AverageCriterion = group.AverageValue,
        //                    Date = DateTime.Now // Suponemos que deseas establecer la fecha actual
        //                };

        //                _context.AnswerCriterionCompanies.Add(newAnswerCriterionCompany);
        //            }
        //        }

        //        // Guardamos los cambios en la base de datos
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al actualizar o crear criterios de la compañía", ex);
        //    }
        //}


        //public async Task<string> SaveAnswerQuestionAsync(FormData formData)
        //{
        //    try
        //    {
        //        var idPerson = long.Parse(SessionHelper.GetValue(User, "IdPerson"));
        //        var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));


        //        var parameters = new[]
        //        {
        //            new SqlParameter("@IdQuestion", SqlDbType.BigInt) { Value = formData.IdQuestion },
        //            new SqlParameter("@IdAnswer", SqlDbType.BigInt) { Value = formData.IdAnswer },
        //            new SqlParameter("@IdCompany", SqlDbType.BigInt) { Value = idCompany },
        //            new SqlParameter("@IdPerson", SqlDbType.BigInt) { Value = idPerson },
        //            new SqlParameter("@IdCriterio", SqlDbType.BigInt) { Value = formData.IdCriterio },
        //            new SqlParameter("@Return", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
        //        };

        //        await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[SaveAnswerQuestion] @IdQuestion, @IdAnswer, @IdCompany, @IdPerson, @IdCriterio, @Return OUT", parameters);

        //        return parameters.Last().Value.ToString();

        //    }
        //    catch (FormatException ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return "Error en el formato";
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return "Error en la base de datos";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return "Error desconocido";
        //    }
        //}

        public async Task<IActionResult> ProcessForm([FromBody] List<FormData> formDataList)
        {
            try
            {
                var idPerson = long.Parse(SessionHelper.GetValue(User, "IdPerson"));
                var idCompany = long.Parse(SessionHelper.GetValue(User, "IdCompany"));

                if (formDataList == null || formDataList.Count == 0)
                {
                    return BadRequest("Debes contestar todas las preguntas");
                }

                List<string> errors = new List<string>();

                foreach (var formData in formDataList)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@IdQuestion", SqlDbType.BigInt) { Value = formData.IdQuestion },
                        new SqlParameter("@IdAnswer", SqlDbType.BigInt) { Value = formData.IdAnswer },
                        new SqlParameter("@IdCompany", SqlDbType.BigInt) { Value = idCompany },
                        new SqlParameter("@IdPerson", SqlDbType.BigInt) { Value = idPerson },
                        new SqlParameter("@IdCriterio", SqlDbType.BigInt) { Value = formData.IdCriterio },
                        new SqlParameter("@Return", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }
                    };

                    await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[SaveAnswerQuestion] @IdQuestion, @IdAnswer, @IdPerson,  @IdCompany, @IdCriterio, @Return OUT", parameters);

                    var procedureOutput = (string)parameters.Last().Value;

                    if (procedureOutput.StartsWith("False:"))
                    {
                        errors.Add(procedureOutput.Substring(6));
                    }
                }

                if (errors.Any())
                {
                    return BadRequest(string.Join("; ", errors));
                }

                return Ok("Formulario procesado correctamente");
            }
            catch (FormatException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }


}
