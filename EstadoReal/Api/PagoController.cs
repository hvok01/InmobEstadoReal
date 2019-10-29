using System;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EstadoReal.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public PagoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/Pago
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var pagosByPropietario = contexto.Pagos.Include(x => x.Contrato)
                    .ThenInclude(e => e.Inmueble)
                    .ThenInclude(z => z.Duenio)
                    .Where(x => x.EstadoPago == 1 && x.Contrato.Inmueble.Duenio.Correo == usuario).ToList();
                return Ok(pagosByPropietario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Pago/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var pagosById = contexto.Pagos
                .Include(i => i.Contrato)
                .ThenInclude(e => e.Inmueble)
                .ThenInclude(p => p.Duenio)
                .Where(x => x.EstadoPago == 1 && x.IdPago == id).ToList();
                return Ok(pagosById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Pago
        [HttpPost]
        public async Task<IActionResult> Post(Pago entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Pagos.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdPago }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Pago/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Pago entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdPago = id;
                    contexto.Pagos.Update(entidad);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Pagos.FirstOrDefault(x => x.EstadoPago == 1 && x.IdPago == id);

                if (entidad != null)
                {
                    entidad.EstadoPago = 0;
                    contexto.Pagos.Update(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
