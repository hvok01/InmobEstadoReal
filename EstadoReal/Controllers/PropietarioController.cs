using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    
    public class PropietarioController : Controller
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly IRepositorioEmpleado empleado;
        private readonly IRepositorioInmueble inmueble;
        private readonly IRepositorioContrato contrato;
        private readonly IRepositorioInquilino inquilino;
        private readonly IRepositorioPago pago;

        public PropietarioController(IRepositorioPropietario repositorio, IRepositorioEmpleado empleado, IRepositorioInmueble inmueble , IRepositorioContrato contrato, IRepositorioInquilino inquilino, IRepositorioPago pago)
        {
            this.repositorio = repositorio;
            this.empleado = empleado;
            this.inmueble = inmueble;
            this.contrato = contrato;
            this.inquilino = inquilino;
            this.pago = pago;
        }

        [Authorize(Policy = "Empleado")]
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var empleadoNombre = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.empleado = empleado.ObtenerPorCorreo(empleadoNombre);

            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        [Authorize(Policy = "Empleado")]
        public ActionResult Details(int id)
        {
            try
            {
                var prop = repositorio.ObtenerPorId(id);
                return View(prop);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Propietario/Create
        [Authorize(Policy = "Empleado")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Empleado")]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                var existeCorreoPropietario = repositorio.ObtenerPorCorreo(propietario.Correo);

                if (ModelState.IsValid && existeCorreoPropietario == null)
                {
                    if (empleado.ObtenerPorCorreo(propietario.Correo) != null)
                    {
                        //el correo ya exite champ
                        return View();
                    }
                    else
                    {
                        propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: propietario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                        repositorio.Alta(propietario);
                        if (TempData.ContainsKey("Id"))
                            ViewBag.Id = TempData["Id"];
                        ViewBag.MensajeError = null;
                        ViewBag.Exito = "Propietario registrado con exito";
                        return View();
                    }
                }
                else
                    if (TempData.ContainsKey("Id"))
                        ViewBag.Id = TempData["Id"];
                    ViewBag.MensajeError = "No che, sabes que te faltó algo";
                    return View();
            }
            catch (Exception ex)
            {
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Propietario/Edit/5
        [Authorize(Policy = "Empleado")]
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var prop = repositorio.ObtenerPorId(id);
            return View(prop);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Empleado")]
        public ActionResult Edit(Propietario propietario)
        {
            ViewBag.Id = propietario.IdPropietario;

            try
            {
                var prop = repositorio.ObtenerPorId(propietario.IdPropietario);

                TempData["Nombre"] = propietario.Nombre;
                if (ModelState.IsValid && propietario.Nombre != "" && propietario.Clave != "")
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Modificacion(propietario);
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Propietario editado con exito";
                    return View(prop);
                }
                else
                    ViewBag.MensajeError = "No che, sabes que te faltó algo";
                    return View(prop);
            }
            catch (Exception ex)
            {
                var prop = repositorio.ObtenerPorId(propietario.IdPropietario);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View(prop);
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy = "Empleado")]
        public ActionResult Delete(int id)
        {
            try
            {
                var prop = repositorio.ObtenerPorId(id);
                return View(prop);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Empleado")]
        public ActionResult Delete(Propietario propietario)
        {
            try
            {
                repositorio.Baja(propietario.IdPropietario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        [Authorize(Policy = "Empleado")]
        public ActionResult ListPropietario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Empleado")]
        public ActionResult ListPropietario(string nombre, string apellido)
        {
            try
            {
                if (ModelState.IsValid && !nombre.Equals("") && !apellido.Equals(""))
                {
                    var propietarios = repositorio.ObtenerPorNombreApellido(nombre, apellido);
                    if (propietarios.Count() == 0)
                    {
                        ViewBag.Error = "No se encontraron resultados";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "";
                        return View(propietarios);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontraron resultados";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se encontraron resultados";
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        [Authorize(Policy="Propietario")]
        public ActionResult VistaPropietarioIndex()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var nombrePropietario = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.propietario = repositorio.ObtenerPorCorreo(nombrePropietario);
            ViewBag.Inmuebles = inmueble.ObtenerPorIdPropietario(ViewBag.propietario.IdPropietario);

            return View();
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult EditarPropietario(int id)
        {
            ViewBag.id = id;
            var prop = repositorio.ObtenerPorId(id);
            return View(prop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Propietario")]
        public ActionResult EditarPropietario(Propietario propietario)
        {
            ViewBag.Id = propietario.IdPropietario;

            try
            {
                var prop = repositorio.ObtenerPorId(propietario.IdPropietario);

                TempData["Nombre"] = propietario.Nombre;
                if (ModelState.IsValid && propietario.Nombre != "" && propietario.Clave != "")
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Modificacion(propietario);
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Propietario editado con exito";
                    return View(prop);
                }
                else
                    ViewBag.MensajeError = "No che, sabes que te faltó algo";
                return View(prop);
            }
            catch (Exception ex)
            {
                var prop = repositorio.ObtenerPorId(propietario.IdPropietario);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View(prop);
            }
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult ListarPropiedades(int id)
        {
            ViewBag.id = id;
            var inmuebles = inmueble.ObtenerPorIdPropietario(id);
            if (inmuebles.Count > 0)
            {
                ViewBag.Inmueble = inmueble.ObtenerPorIdPropietario(id);
                ViewBag.Error = "";
                return View();
            } else
            {
                ViewBag.Error = "No encontramos inmuebles a tu nombre";
                return View();
            }
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult EditarDisponibilidad(int id)
        {
            ViewBag.Inmueble = inmueble.ObtenerPorId(id);
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Propietario")]
        public ActionResult EditarDisponibilidad(Inmueble inmue)
        {
            var inmo = inmueble.ActualizarDisponibilidad(inmue.IdInmueble, inmue.Disponibilidad);

            ViewBag.Inmueble = inmueble.ObtenerPorId(inmue.IdInmueble);

            if (inmo != -1)
            {
                ViewBag.MensajeError = null;
                ViewBag.Exito = "Cambios realizados con exito";
                return View();
            }
            else
            {
                ViewBag.MensajeError = "Error";
                return View();
            }
            
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult VerInquilinos(int id)
        {
            ViewBag.Inmuebles = inmueble.ObtenerPorIdPropietario(id);
            ViewBag.Contratos = contrato.ObtenerTodos();
            ViewBag.Inquilinos = inquilino.ObtenerTodos();
            return View();
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult VerPagos(int id)
        {
            ViewBag.Inmuebles = inmueble.ObtenerPorIdPropietario(id);
            ViewBag.Contratos = contrato.ObtenerTodos();
            ViewBag.pagos = pago.ObtenerTodos();
            return View();
        }

        [Authorize(Policy = "Propietario")]
        public ActionResult VerContratos(int id)
        {
            ViewBag.Inmuebles = inmueble.ObtenerPorIdPropietario(id);
            ViewBag.Contratos = contrato.ObtenerTodos();
            return View();
        }
    }
}