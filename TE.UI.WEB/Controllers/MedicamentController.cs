using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [Route("api/medicament")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly MedicamentService _service;

        public MedicamentController(MedicamentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicament>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicament>> GetById(long id)
        {
            var medicament = await _service.GetByIdAsync(id);
            if (medicament == null) return NotFound();
            return Ok(medicament);
        }

        [HttpPost]
        public async Task<ActionResult<Medicament>> Create([FromBody] Medicament medicament)
        {
            var created = await _service.CreateAsync(medicament);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Medicament medicament)
        {
            if (id != medicament.Id) return BadRequest();

            var updated = await _service.UpdateAsync(medicament);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
