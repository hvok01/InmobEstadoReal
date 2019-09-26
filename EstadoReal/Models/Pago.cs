using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public decimal Monto { get; set; }
        public bool Estado { get; set; }
        public string Fecha { get; set; }
        public int NroPago { get; set; }
        public int IdContrato { get; set; }
    }
}
