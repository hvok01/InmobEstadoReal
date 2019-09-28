using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        [Required]
        public string InicioContrato { get; set; }
        [Required]
        public string FinContrato { get; set; }
        [Required]
        public decimal Deudas { get; set; }
        [Required]
        public int IdInquilino { get; set; }
        [Required]
        public int IdInmueble { get; set; }

        public byte EstadoContrato { get; set; }
    }
}
