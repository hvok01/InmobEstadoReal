using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Inmueble
    {
        [Key]
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public bool UsoResidencial { get; set; }
        public string Tipo { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
        public int IdPropietario { get; set; }
    }
}
