using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        Inmueble ObtenerPorIdPropietario(int id);
    }
}
