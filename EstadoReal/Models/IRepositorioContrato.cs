using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        Contrato ObtenerPorIdInquilino(int id);

        Contrato ObtenerPorIdInmueble(int id);

        IList<Contrato> BuscarEntreFechas(string fechaA, string fechaB);

    }
}
