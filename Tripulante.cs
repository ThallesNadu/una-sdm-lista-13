using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmericanAirLinesApi.Models
{
    public class Tripulante
    {
        public int Id { get; set; }
        public string Nome { get; set; }   
        public string Funcao { get; set; }
        public string NumeroLicenca { get; set; }
    }
}