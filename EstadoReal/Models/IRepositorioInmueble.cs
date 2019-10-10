using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> ObtenerPorIdPropietario(int id);
        IList<Inmueble> ObtenerDisponiblesPorIdPropietario(int id);
        IList<Inmueble>  ObtenerDisponibles();
        IList<Contrato> ObtenerContratos(int id);
    }
}
