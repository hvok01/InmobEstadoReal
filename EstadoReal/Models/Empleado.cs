using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int Dni { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Correo { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        public byte EstadoEmpleado { get; set; }

    }
}
