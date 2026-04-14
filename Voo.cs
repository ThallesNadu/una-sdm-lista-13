using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmericanAirLinesApi.Models
{
    public class Voo
    {
        public int Id { get; set; }
        public string CodigoVoo { get; set; }  
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int AeronaveId { get; set; }
        public string Status { get; set; }

    }
}