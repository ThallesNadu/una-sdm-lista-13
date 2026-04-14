using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirLinesApi.Data;
using AmericanAirLinesApi.Models;

namespace AmericanAirLinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetAll()
        {
            var reservas = await _context.Reservas.ToListAsync();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetById(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            return Ok(reserva);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Reserva reserva)
        {
            var voo = await _context.Voos.FirstOrDefaultAsync(v => v.Id == reserva.VooId);
            if (voo == null)
                return BadRequest("O voo informado não existe.");

            var aeronave = await _context.Aeronaves.FirstOrDefaultAsync(a => a.Id == voo.AeronaveId);
            if (aeronave == null)
                return BadRequest("A aeronave vinculada ao voo não existe.");

            var quantidadeReservas = await _context.Reservas.CountAsync(r => r.VooId == reserva.VooId);

            if (quantidadeReservas >= aeronave.CapacidadePassageiros)
            {
                return BadRequest("Voo lotado. Não é possível realizar novas reservas.");
            }

            decimal valorBaseFicticio = 200.00m;
            decimal taxaJanela = 0.00m;

            if (!string.IsNullOrWhiteSpace(reserva.Assento))
            {
                var assento = reserva.Assento.Trim().ToUpper();

                if (assento.EndsWith("A") || assento.EndsWith("F"))
                {
                    taxaJanela = 50.00m;
                    Console.WriteLine("Assento na janela reservado com sucesso!");
                }
            }

            decimal valorFinalFicticio = valorBaseFicticio + taxaJanela;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, new
            {
                mensagem = "Reserva criada com sucesso.",
                reserva.Id,
                reserva.VooId,
                reserva.NomePassageiro,
                reserva.Assento,
                ValorBase = valorBaseFicticio,
                TaxaJanela = taxaJanela,
                ValorFinal = valorFinalFicticio
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Reserva reserva)
        {
            if (id != reserva.Id)
                return BadRequest("ID da rota diferente do ID da reserva.");

            var reservaExistente = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (reservaExistente == null)
                return NotFound("Reserva não encontrada.");

            var voo = await _context.Voos.FirstOrDefaultAsync(v => v.Id == reserva.VooId);
            if (voo == null)
                return BadRequest("O voo informado não existe.");

            var aeronave = await _context.Aeronaves.FirstOrDefaultAsync(a => a.Id == voo.AeronaveId);
            if (aeronave == null)
                return BadRequest("A aeronave vinculada ao voo não existe.");

            var quantidadeReservas = await _context.Reservas.CountAsync(r => r.VooId == reserva.VooId && r.Id != id);

            if (quantidadeReservas >= aeronave.CapacidadePassageiros)
            {
                return BadRequest("Voo lotado. Não é possível realizar novas reservas.");
            }

            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}