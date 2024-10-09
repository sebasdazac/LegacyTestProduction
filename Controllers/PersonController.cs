using LegacyTest.Models;
using LegacyTest.Models.PersonAux;
using LegacyTest.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LegacyTest.Controllers
{
    public class PersonController : Controller
    {

        private readonly LegacyDBContext _context;
        private readonly Crypto crypto = new();


        public PersonController(LegacyDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            int id = Int32.Parse(SessionHelper.GetValue(User, "IdPerson"));

            var user = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var profileModel = new UpdateProfileModel
            {
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email
            };

            return Ok(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            int id = Int32.Parse(SessionHelper.GetValue(User, "IdPerson"));

            var user = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.FirstName;
            user.Surname = model.LastName;
            user.Email = model.Email;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                int id = Int32.Parse(SessionHelper.GetValue(User, "IdPerson"));

                var user = await _context.People.FirstOrDefaultAsync(x => x.Id == id 
                                                                       && x.Pswd.Equals(crypto.Encrypt(model.CurrentPassword)));
                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                if (user.Pswd != crypto.Encrypt(model.CurrentPassword))
                {
                    return BadRequest("La contraseña actual es incorrecta.");
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return BadRequest("Las nuevas contraseñas no coinciden.");
                }

                user.Pswd = crypto.Encrypt(model.NewPassword);
                await _context.SaveChangesAsync();

                return Ok("Contraseña cambiada correctamente.");
            }
            catch (FormatException e)
            {

                return BadRequest($"Ocurrió un error al cambiar la contraseña. Por favor, intenta nuevamente.{e.Message}");
            }
            catch (SqlException e)
            {

                return BadRequest($"Ocurrió un error al cambiar la contraseña. Por favor, intenta nuevamente. {e.Message}");
            }
            catch (Exception e)
            {              

                return BadRequest($"Ocurrió un error al cambiar la contraseña. Por favor, intenta nuevamente. {e.Message}");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            int id = Int32.Parse(SessionHelper.GetValue(User, "IdPerson"));

            var user = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.People.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }



}

