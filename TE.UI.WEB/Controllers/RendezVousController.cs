using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [Route("api/rendezvous")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly RendezVousService _rendezVousService;
        public RendezVousController(RendezVousService  rendezVousService)
        {
            _rendezVousService = rendezVousService;
        }
        [HttpGet("patient/{idPatient}")]
        public IActionResult getRendezVousPerPatient(long idPatient) {
            var result = _rendezVousService.getRendezVousPerPatient(idPatient);
            if (result == null)
            {
                return NotFound("probleme patient");

            } return Ok(result);

        }
        [Authorize(Roles = "Medecin")]
        [HttpGet("medecin")]
        public IActionResult getRendezVousPerMedecin()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            long medecinId = long.Parse(userIdClaim.Value);
            var result = _rendezVousService.getRendezVousPerMedecin(medecinId);
            if (result == null)
            {
                return NotFound("probleme medcin");

            }
            return Ok(result);

        }
        [HttpPut("confirme/{id}")]
        public IActionResult confirmerRdv(long id)
        {
            var result = _rendezVousService.confirmerRendezVous(id);
            if (!result)
            {
                return NotFound("probleme rdv");

            }
            return Ok("rendez vous confirmé avec succes ");

        }
        [HttpPut("annule/{id}")]
        public IActionResult annulerRdv(long id)
        {
            var result = _rendezVousService.annulerRendezVous(id);
            if (!result)
            {
                return NotFound("probleme rdv");

            }
            return Ok("rendez vous annulé avec succes ");

        }
    }
}
