using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioEmpleado : IRepositorio<Empleado>
    {
        Empleado ObtenerPorCorreo(string correo);

    }
}
