using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmericanAirLinesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AmericanAirLinesApi.Data
{
    public class AppDbContext : DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Aeronave> Aeronaves { get; set; }
        public DbSet<Tripulante> Tripulantes { get; set; }
         public DbSet<Voo> Voos { get; set; }
         public DbSet<Reserva> Reservas { get; set; }
    }
}