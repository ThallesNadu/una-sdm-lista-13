using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirLinesApi.Data;

namespace AmericanAirLinesApi.Controllers
{
    [ApiController]
    [Route("api/radar")]
    public class FlightRadarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightRadarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("proximos-destinos")]
        public async Task<ActionResult> GetProximosDestinos()
        {
            Thread.Sleep(2000);

            var destinos = await _context.Voos
                .Where(v => v.Status != "Finalizado" && v.Status != "Cancelado")
                .GroupBy(v => v.Destino)
                .Select(g => new
                {
                    Destino = g.Key,
                    QuantidadeVoos = g.Count()
                })
                .OrderByDescending(x => x.QuantidadeVoos)
                .ThenBy(x => x.Destino)
                .ToListAsync();

            return Ok(destinos);
        }
    }
}