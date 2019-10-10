using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        Pago ObtenerPorIdContrato(int id);
        IList<Pago> ObtenerPagosPorContrato(int id);
    }
}
