using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly FicheMedicaleService _ficheMedicaleService;
        private readonly AgendaService _agendaService;
        public PatientController(FicheMedicaleService ficheMedicalService,AgendaService agendaService) {
        _ficheMedicaleService = ficheMedicalService;
            _agendaService = agendaService;
        }    
        [HttpPut("{patientId}/fichemedicale")]
        public IActionResult UpdateFicheByPatient(int patientId, [FromBody] FicheMedical updatedFiche)
        {
            var fiche = _ficheMedicaleService.UpdateFicheForPatient(patientId, updatedFiche);

            if (fiche == null)
                return NotFound("Fiche médicale ou patient introuvable.");

            return Ok(fiche);
        }
        [HttpPost("rendezvous/{patientId}/agenda/{agendaId}")]
        public IActionResult ReserverRendezVous(
            long patientId,
            long agendaId,
            [FromBody] RendezVous request
            )
        {
            var result = _agendaService.ReserverRendezVous(patientId, agendaId, request);
            if (result == null)
                return BadRequest("Créneau indisponible ou utilisateur introuvable.");

            return Ok(result);
        }


    }
}
