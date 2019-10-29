using System;
using System.Collections.Generic;
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
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InquilinoController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/Inquilino
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var inquilinosByPropietario = contexto.Contratos.Include(x => x.Inquilino)
                    .Include(i => i.Inmueble)
                    .ThenInclude(p => p.Duenio).Where(p => p.Inmueble.Duenio.Correo == usuario)
                    .Select(x => x.Inquilino).Where(x => x.EstadoInquilino == 1).ToList();

                return Ok(inquilinosByPropietario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var inquilinosById = contexto.Contratos.Include(x => x.Inquilino)
                    .Include(i => i.Inmueble)
                    .ThenInclude(p => p.Duenio).Where(p => p.Inmueble.Duenio.Correo == usuario)
                    .Select(x => x.Inquilino).FirstOrDefault(x => x.EstadoInquilino == 1 && x.IdInquilino == id);

                return Ok(inquilinosById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Inquilino
        [HttpPost]
        public async Task<IActionResult> Post(Inquilino entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Inquilinos.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.IdInquilino }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Inquilino/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inquilino entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.IdInquilino = id;
                    contexto.Inquilinos.Update(entidad);
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
                var entidad = contexto.Inquilinos.FirstOrDefault(x => x.EstadoInquilino == 1 && x.IdInquilino == id);

                if (entidad != null)
                {
                    entidad.EstadoInquilino = 0;
                    contexto.Inquilinos.Update(entidad);
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
