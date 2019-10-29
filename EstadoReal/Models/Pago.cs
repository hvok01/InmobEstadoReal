using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public byte Pagado { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public int NroPago { get; set; }

        public byte EstadoPago { get; set; }
        [Required]
        public int IdContrato { get; set; }

        [ForeignKey("IdContrato")]
        public Contrato Contrato { get; set; }
    }
}
