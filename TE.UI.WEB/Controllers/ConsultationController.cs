using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [Route("api/consultation")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly ConsultationService _consultationService;
        public ConsultationController(ConsultationService consultationService) {
            _consultationService = consultationService;
        }
        [HttpPost("demarrer/{rdvId}")]
        public IActionResult DemarrerConsultation(long rdvId)
        {
            var consultation = _consultationService.DemarrerConsultation(rdvId);

            if (consultation == null)
                return BadRequest("Impossible de démarrer la consultation. Vérifie que le rendez-vous est confirmé.");

            return Ok(consultation);
        }
        [HttpPost("finaliser/{id}")]
        public async Task<IActionResult> FinaliserConsultation(long id)
        {
            var consultation = await _consultationService.FinaliserConsultationAsync(id);

            if (consultation == null)
                return BadRequest("Consultation introuvable ou déjà finalisée.");

            return Ok(new
            {

                message = "Consultation finalisée avec succès.",
                consultation.IdConsultation,
                dureeEnMinutes = consultation.DureeTotale?.TotalMinutes,
                depasseLimite = consultation.DureeTotale?.TotalMinutes > 30
            });
        }
        [HttpPost("{consultationId}/diagnostics")]
        public async Task<IActionResult> AjouterDiagnostics(long consultationId, [FromBody] List<Diagnostic> diagnostics)
        {
            if (diagnostics == null || !diagnostics.Any())
                return BadRequest("La liste des diagnostics est vide.");

            try
            {
                await _consultationService.AjouterDiagnosticsAsync(consultationId, diagnostics);
                return Ok(new { message = "Diagnostics ajoutés avec succès." });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message;
                return BadRequest(new { message = "Erreur lors de l'ajout des diagnostics.", details = inner ?? ex.Message });
            }

        }


    }
}
