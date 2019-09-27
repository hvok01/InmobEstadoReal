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
        [Required]
        public decimal Monto { get; set; }
        [Required]
        public byte Estado { get; set; }
        [Required]
        public string Fecha { get; set; }
        [Required]
        public int NroPago { get; set; }
        [Required]
        public int IdContrato { get; set; }
    }
}
