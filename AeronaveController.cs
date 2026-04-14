

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirLinesApi.Data;
using AmericanAirLinesApi.Models;

namespace AmericanAirLinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AeronavesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AeronavesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aeronave>>> GetAll()
        {
            var itens = await _context.Aeronaves.ToListAsync();
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aeronave>> GetById(int id)
        {
            var item = await _context.Aeronaves.FindAsync(id);

            if (item == null)
                return NotFound("Aeronave não encontrada.");

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Aeronave>> Create(Aeronave aeronave)
        {
            _context.Aeronaves.Add(aeronave);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = aeronave.Id }, aeronave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Aeronave aeronave)
        {
            if (id != aeronave.Id)
                return BadRequest("ID da rota diferente do ID da aeronave.");

            var existe = await _context.Aeronaves.AnyAsync(x => x.Id == id);
            if (!existe)
                return NotFound("Aeronave não encontrada.");

            _context.Entry(aeronave).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Aeronaves.FindAsync(id);

            if (item == null)
                return NotFound("Aeronave não encontrada.");

            _context.Aeronaves.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}