using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Inmueble
    {
        [ForeignKey("Inmueble")]
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
        [Required]
        public decimal Longitud { get; set; }
        [Required]
        public decimal Latitud { get; set; }

        public byte EstadoInmueble { get; set; }
        [Required]
        public int IdPropietario { get; set; }

        [ForeignKey("IdPropietario")]
        public Propietario Duenio { get; set; }

    }
}
