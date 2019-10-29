using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public class Inquilino
    {
        [ForeignKey("Inquilino")]
        [Key]
        public int IdInquilino { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        public string LugarTrabajo { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public long Telefono { get; set; }
        [Required]
        public string NombreGarante { get; set; }
        [Required]
        public int DniGarante { get; set; }

        public byte EstadoInquilino { get; set; }
    }
}
