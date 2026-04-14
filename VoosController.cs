using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirLinesApi.Data;
using AmericanAirLinesApi.Models;

namespace AmericanAirLinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voo>>> GetAll()
        {
            var voos = await _context.Voos.ToListAsync();
            return Ok(voos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voo>> GetById(int id)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound("Voo não encontrado.");

            return Ok(voo);
        }

        [HttpPost]
        public async Task<ActionResult<Voo>> Create(Voo voo)
        {
            var aeronaveExiste = await _context.Aeronaves.AnyAsync(a => a.Id == voo.AeronaveId);
            if (!aeronaveExiste)
                return BadRequest("A aeronave informada não existe.");

            var aeronaveEmTransito = await _context.Voos
                .AnyAsync(v => v.AeronaveId == voo.AeronaveId && v.Status == "Em Voo");

            if (aeronaveEmTransito)
                return Conflict("Aeronave indisponível, encontra-se em trânsito.");

            _context.Voos.Add(voo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = voo.Id }, voo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Voo voo)
        {
            if (id != voo.Id)
                return BadRequest("ID da rota diferente do ID do voo.");

            var vooExistente = await _context.Voos.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (vooExistente == null)
                return NotFound("Voo não encontrado.");

            var aeronaveExiste = await _context.Aeronaves.AnyAsync(a => a.Id == voo.AeronaveId);
            if (!aeronaveExiste)
                return BadRequest("A aeronave informada não existe.");

            var aeronaveEmTransito = await _context.Voos
                .AnyAsync(v =>
                    v.Id != id &&
                    v.AeronaveId == voo.AeronaveId &&
                    v.Status == "Em Voo");

            if (aeronaveEmTransito)
                return Conflict("Aeronave indisponível, encontra-se em trânsito.");

            _context.Entry(voo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] AtualizarStatusVooDto dto)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound("Voo não encontrado.");

            if ((voo.Status == "Finalizado" || voo.Status == "Cancelado") &&
                dto.Status == "Em Voo")
            {
                return BadRequest("Regra de negócio violada: um voo Finalizado ou Cancelado não pode voltar para Em Voo.");
            }

            voo.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Status atualizado com sucesso.",
                voo.Id,
                voo.CodigoVoo,
                voo.Status
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound("Voo não encontrado.");

            _context.Voos.Remove(voo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class AtualizarStatusVooDto
    {
        public string Status { get; set; }
    }
}