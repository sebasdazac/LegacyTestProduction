
using LegacyTest.Models;
using LegacyTest.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LegacyTest.Controllers
{
    public class FormController : Controller
    {
        private readonly LegacyDBContext _context;

        public FormController(LegacyDBContext context)
        {
            _context = context;
        }

        public IActionResult Plan()
        {
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }

        public IActionResult Question()
        {
            return View();
        }

        public IActionResult Answer()
        {
            return View();
        }

        public IActionResult Criterion()
        {
            return View();
        }

        public IActionResult Clasification()
        {
            return View();
        }

        public IActionResult Characterization()
        {
            return View();
        }

        public async Task<IActionResult> GetAllPlan()
        {
            try
            {
                var data = await _context.Plans.ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var data = await _context.Forms.ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetAllQuestion()
        {
            try
            {
                var data = await _context.Questions.ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetAllAnswer()
        {
            try
            {
                var data = await _context.Answers.ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetAllCriterion()
        {
            try
            {
                var data = await _context.Criteria
                                    .Select(c => new
                                    {
                                        Id = c.Id,
                                        IdForm = c.IdForm,
                                        NameForm = c.IdFormNavigation.NameForm,
                                        CriterionText = c.CriterionText
                                    })
                                    .ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }


        public async Task<IActionResult> GetAllCharacterization()
        {
            try
            {
                var data = await _context.CriterionCharacterizations
                                    .Select(c => new
                                    {
                                        Id = c.Id,
                                        IdCriterion1 = c.IdCriterion1,
                                        CriterionText1 = c.IdCriterion1Navigation.CriterionText,
                                        Clasification1 = c.Clasification1,
                                        Min1 = c.Min1,
                                        Max1 = c.Max1,
                                        IdCriterion2 = c.IdCriterion2,
                                        CriterionText2 = c.IdCriterion2Navigation.CriterionText,
                                        Clasification2 = c.Clasification2,
                                        Min2 = c.Min2,
                                        Max2 = c.Max2,
                                        Characterization = c.Characterization
                                    })
                                    .ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }


        public async Task<IActionResult> GetAllClasification()
        {
            try
            {
                var data = await _context.CriterionClasifications
                                    .Select(c => new
                                    {
                                        Id = c.Id,
                                        IdCriterion = c.IdCriterion,
                                        CriterionText = c.IdCriterionNavigation.CriterionText,
                                        Clasification = c.Clasification,
                                        Min = c.Min,
                                        Max = c.Max
                                    })
                                    .ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }
        public async Task<IActionResult> GetListForm()
        {
            try
            {
                var data = await _context.Forms
                                            .Select(f => new
                                            {
                                                Id = f.Id,
                                                NameForm = f.NameForm
                                            })
                                            .ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetListQuestionParent()
        {
            try
            {
                var data = await _context.Questions
                                           .Select(q => q.Id)
                                           .ToListAsync();

                return Json(data);

            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }

        public async Task<IActionResult> GetListCriterion()
        {
            try
            {
                var data = await _context.Criteria
                                            .Select(f => new
                                            {
                                                Id = f.Id,
                                                CriterionText = f.CriterionText
                                            })
                                            .ToListAsync();

                return Json(data);
            }
            catch (SqlException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (FormatException ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
            catch (Exception ex)
            {
                return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
            }
        }








        //public async Task<IActionResult> SaveOrUpdatePlan([FromForm] Plan plan)
        //{
        //    try
        //    {
        //        var param = new SqlParameter[] {
        //            new SqlParameter {ParameterName = "@P_CODIGO", SqlDbType = SqlDbType.VarChar, Value = plan.Codigo},
        //            new SqlParameter {ParameterName = "@P_NOMBRE", SqlDbType = SqlDbType.VarChar, Value=plan.Nombre},
        //            new SqlParameter {ParameterName = "@P_VALOR", SqlDbType = SqlDbType.Int, Value= plan.Valor},
        //            new SqlParameter {ParameterName = "@P_ESTADO", SqlDbType = SqlDbType.VarChar, Value=plan.Estado},
        //            new SqlParameter {ParameterName = "@Retorno", SqlDbType = SqlDbType.VarChar, Value=""}
        //        };

        //        var data = await _context.Database.ExecuteSqlRawAsync($"Pr_Gestionar_Plan @P_CODIGO, @P_NOMBRE, @P_VALOR, @P_ESTADO, @Retorno", param);

        //        await _context.SaveChangesAsync();

        //    }
        //    catch (SqlException ex)
        //    {

        //        return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
        //    }

        //    catch (FormatException ex)
        //    {

        //        return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
        //    }


        //    catch (Exception ex)
        //    {

        //        return Json(new { data = new { ErrorMessage = ex.Message, Success = false } });
        //    }

        //    return Json(new { data = new { Success = true } });
        //}

    }
}
