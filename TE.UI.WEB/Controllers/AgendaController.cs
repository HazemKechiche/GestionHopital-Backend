using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TE.Core.Domain;
using TE.Core.Services;
using TE.Data;

namespace TE.UI.WEB.Controllers
{
    [Route("api/agenda")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly AgendaService _agendaService;
        private readonly TEContext _context;
        public AgendaController(AgendaService agendaService, TEContext tEContext )
        {
            
            _agendaService = agendaService;
            _context = tEContext;
        }

        [HttpPost("{medecinId}/add")]
        public IActionResult AddAgenda(long medecinId, [FromBody] Agenda agenda)
        {
            return Ok(_agendaService.addMedecinAgenda(medecinId, agenda));

        }
        ///////////////////////////
        //////// ajouter disponibilite pour medecin dans un jour choisis 
        ///
        ///// POST api/agenda/medecins/{medecinId}/disponibilites
        [Authorize(Roles = "Medecin")]
        [HttpPost("disponibilites")]
        public IActionResult CreerDisponibilites([FromBody] DisponibiliteCreateDto dto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Utilisateur non authentifié." });

                long medecinId = long.Parse(userIdClaim.Value);

                _agendaService.CreerDisponibilites30Min(
                    medecinId,
                    dto.Date,
                    dto.HeureDebut,
                    dto.HeureFin,
                    dto.Description);

                return Ok(new { message = "Disponibilités créées avec succès." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = $"Paramètre invalide : {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue : " + ex.Message });
            }
        }

        // GET api/agenda/medecins/{medecinId}/disponibilites?date=2025-05-19
        [HttpGet("medecins/{medecinId}/disponibilites")]
        public IActionResult GetDisponibilites(
            long medecinId,
            [FromQuery] DateTime date)
        {
            var dispo = _agendaService.GetDispoMedecin(medecinId, date);
            return Ok(dispo);
        }

        [Authorize(Roles = "Medecin")]
        [HttpGet("mes-agendas")]
        public IActionResult GetMesAgendas()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Utilisateur non authentifié." });

                long medecinId = long.Parse(userIdClaim.Value);

                var agendas = _context.agendas
                    .Where(a => a.UtilisateurId == medecinId)
                    .OrderBy(a => a.Date)
                    .ThenBy(a => a.HeureDebut)
                    .Select(a => new {
                        Id = a.Id,
                        Date = a.Date,
                        HeureDebut = a.HeureDebut.ToString(@"hh\:mm\:ss"),
                        HeureFin = a.HeureFin.ToString(@"hh\:mm\:ss"),
                        Description = a.Description,
                        IsBooked = a.IsBooked,
                        Type = a.Type.ToString()
                    })
                    .ToList();

                return Ok(agendas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue : " + ex.Message });
            }
        }

        [Authorize(Roles = "Medecin")]
        [HttpDelete("{id}")]
        public IActionResult SupprimerDisponibilite(long id)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Utilisateur non authentifié." });

                long medecinId = long.Parse(userIdClaim.Value);

                var agenda = _context.agendas.FirstOrDefault(a => a.Id == id && a.UtilisateurId == medecinId);
                if (agenda == null)
                    return NotFound(new { message = "Agenda non trouvé." });

                // Vérifier que ce n'est pas un RDV déjà réservé
                if (agenda.IsBooked)
                    return BadRequest(new { message = "Impossible de supprimer un rendez-vous déjà réservé." });

                _context.agendas.Remove(agenda);
                _context.SaveChanges();

                return Ok(new { message = "Disponibilité supprimée avec succès." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue : " + ex.Message });
            }
        }

        // Optionnel : pour récupérer les détails d'un agenda spécifique
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetAgenda(long id)
        {
            try
            {
                var agenda = _context.agendas
                    .Include(a => a.Utilisateur)
                    .FirstOrDefault(a => a.Id == id);

                if (agenda == null)
                    return NotFound(new { message = "Agenda non trouvé." });

                var result = new
                {
                    Id = agenda.Id,
                    Date = agenda.Date,
                    HeureDebut = agenda.HeureDebut.ToString(@"hh\:mm\:ss"),
                    HeureFin = agenda.HeureFin.ToString(@"hh\:mm\:ss"),
                    Description = agenda.Description,
                    IsBooked = agenda.IsBooked,
                    Type = agenda.Type.ToString(),
                    MedecinNom = agenda.Utilisateur?.Name,
                    MedecinPrenom = agenda.Utilisateur?.Surname
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue : " + ex.Message });
            }
        }

    }
    // Dans ton AgendaController, ajoute ces méthodes :

    public class DisponibiliteCreateDto
    {
        public DateTime Date { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        public string Description { get; set; }
    }

}
