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
        public string InicioContrato { get; set; }
        public string FinContrato { get; set; }
        public decimal Deudas { get; set; }
        public int IdInquilino { get; set; }
        public int IdInmueble { get; set; }
    }
}
