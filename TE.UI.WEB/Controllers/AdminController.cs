using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _as;
        private readonly IWebHostEnvironment _env;

        public AdminController(AdminService adminService, IWebHostEnvironment env)
        {
            _as = adminService;
            _env = env;
        }
        [HttpGet("get/pending")]
        public IActionResult getPendingExams()
        {
            return Ok(_as.ExamenEnAttente());
        }
        [HttpPut("{idA}/{idE}/exam")]
        public IActionResult doExam(long idA, long idE)
        {
            Examen exam = _as.FaireExamen(idE, idA);
            if (exam == null)
            {
                return BadRequest("cannot fin exam or admin");
            }
            return Ok(exam);

        }



        [HttpPut("mettreAJourResultat")]
        public async Task<IActionResult> MettreAJourResultat([FromForm] ResultatExamenDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _as.MettreAJourResultatExamenAsync(
                dto.ExamenId,
                dto.LignesResultat,
                dto.Commentaire,
                dto.Images
            );

            if (!success)
                return BadRequest("Erreur lors de la mise à jour de l'examen.");

            return Ok("Résultat d'examen mis à jour avec succès.");
        }


    }
    public class ResultatExamenDto
    {
        [Required]
        public long ExamenId { get; set; }

        [Required]
        public List<string>? LignesResultat { get; set; }

        public string? Commentaire { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}
