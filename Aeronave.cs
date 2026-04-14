using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmericanAirLinesApi.Models
{
    public class Aeronave 
    {
        public int Id { get; set; }
        public string Modelo { get; set; }     
        public string CodigoCauda { get; set; }
        public int CapacidadePassageiros { get; set; } 
    }
}