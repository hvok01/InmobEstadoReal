using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        Inquilino ObtenerPorCorreo(string correo);
        IList<Inquilino> ObtenerPorNombreApellido(string nombre, string apellido);
        IList<Inquilino> ObtenerPorContratoId(int id);
    }
    
}
