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
        [Required]
        public string Direccion { get; set; }
        [Required]
        public byte UsoResidencial { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public byte Disponibilidad { get; set; }

        public byte EstadoInmueble { get; set; }
        [Required]
        public int IdPropietario { get; set; }
    }
}
