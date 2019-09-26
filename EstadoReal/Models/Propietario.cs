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
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Correo { get; set; }
        public int Telefono { get; set; }
        public string Clave { get; set; }
    }
}
