using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstadoReal.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        Inquilino ObtenerPorCorreo(string correo);
    }
    
}
