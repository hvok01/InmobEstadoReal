using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario ObtenerPorCorreo(string correo);

        IList<Propietario> ObtenerPorNombreApellido(string nombre, string apellido);

    }
}
