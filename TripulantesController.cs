using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirLinesApi.Data;
using AmericanAirLinesApi.Models;

namespace AmericanAirLinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripulantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripulantesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tripulante>>> GetAll()
        {
            var itens = await _context.Tripulantes.ToListAsync();
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tripulante>> GetById(int id)
        {
            var item = await _context.Tripulantes.FindAsync(id);

            if (item == null)
                return NotFound("Tripulante não encontrado.");

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Tripulante>> Create(Tripulante tripulante)
        {
            _context.Tripulantes.Add(tripulante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tripulante.Id }, tripulante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Tripulante tripulante)
        {
            if (id != tripulante.Id)
                return BadRequest("ID da rota diferente do ID do tripulante.");

            var existe = await _context.Tripulantes.AnyAsync(x => x.Id == id);
            if (!existe)
                return NotFound("Tripulante não encontrado.");

            _context.Entry(tripulante).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Tripulantes.FindAsync(id);

            if (item == null)
                return NotFound("Tripulante não encontrado.");

            _context.Tripulantes.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}