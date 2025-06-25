using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TE.Core.Domain;
using TE.Core.Services;
using static TE.Core.Services.MedecinService;

namespace TE.UI.WEB.Controllers
{
    [Route("api/medecin")]
    [ApiController]
    public class MedecinController : ControllerBase
    {
        private readonly MedecinService _medecinService;
        
        public MedecinController (MedecinService medecinService)
        {
            _medecinService = medecinService;
        }
        [HttpGet("getall")]
        public IActionResult Index() { 
            IList<Medecin> medecins =_medecinService.getAllMedecins();
            if (medecins.Count() == 0)
            {
                return NotFound("there is not medecins");

            }
            return Ok(medecins);
        }
        [Authorize(Roles = "Medecin")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var profile = await _medecinService.GetProfile();
                return Ok(profile);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetProfile: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "Medecin")]
        [HttpGet("patients-finalised")]
        public async Task<ActionResult<List<PatientDto>>> GetPatientsWithFinalisedConsultations()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            long medecinId = long.Parse(userIdClaim.Value);

            var patients = await _medecinService.GetPatientsWithFinalisedConsultationsAsync(medecinId);
            return Ok(patients);
        }


    }
}
