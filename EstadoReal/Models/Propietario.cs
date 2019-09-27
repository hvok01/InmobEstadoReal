using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Propietario
    {
        [Key]
        public int IdPropietario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public int Telefono { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
