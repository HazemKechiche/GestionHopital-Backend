using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [Route("api/ordenance")]
    [ApiController]
    public class OrdenanceController : ControllerBase
    {
        private readonly OrdenanceService _service;

        public OrdenanceController(OrdenanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var ordonnance = await _service.GetByIdAsync(id);
            if (ordonnance == null) return NotFound();
            return Ok(ordonnance);
        }

        // POST: api/Ordenance/consultation/5
        [HttpPost("ajouter/{consultationId}")]
        public async Task<IActionResult> CreateOrdonnance(long consultationId, [FromBody] Ordenance ordonnance)
        {
            if (ordonnance == null)
                return BadRequest("L'ordonnance est vide.");

            var result = await _service.CreateAsync(consultationId, ordonnance);

            if (result == null)
                return NotFound($"Aucune consultation trouvée avec l'ID {consultationId}.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Ordenance ordonnance)
        {
            if (id != ordonnance.IdOrdonnance) return BadRequest("ID mismatch");
            var result = await _service.UpdateAsync(ordonnance);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }

}
