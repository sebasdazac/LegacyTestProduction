using LegacyTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LegacyTest.Controllers
{
    public class RecomendationController : Controller
    {
        private readonly LegacyDBContext _context;

        public RecomendationController(LegacyDBContext context)
        {
            _context = context;

        }

        [HttpPost]
        public async Task<IActionResult> GetAlls(long idCriterion1, long idCriterion2, string characterization)
        {
            try
            {
                var item = await _context.CriterionCharacterizations
                                        .Where(x => x.IdCriterion1 == idCriterion1 &&
                                                     x.IdCriterion2 == idCriterion2 &&
                                                     x.Characterization.Equals(characterization)
                                               ).FirstOrDefaultAsync();

                var data = await _context.CharacterizationRecomendations
                                        .Where(x => x.IdCharacterization == item.Id
                                               ).ToListAsync();

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
