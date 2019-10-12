using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioEmpleado empleadosRepo;
        private readonly IRepositorioPropietario propietario;
        private readonly IRepositorioInquilino inquilino;

        public InmuebleController(IRepositorioInmueble repositorio, IRepositorioEmpleado empleadosRepo, IRepositorioPropietario propietario, IRepositorioInquilino inquilino)
        {
            this.repositorio = repositorio;
            this.empleadosRepo = empleadosRepo;
            this.propietario = propietario;
            this.inquilino = inquilino;
        }

        // GET: Inmueble
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var empleadoNombre = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.empleado = empleadosRepo.ObtenerPorCorreo(empleadoNombre);

            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var inmueble = repositorio.ObtenerPorId(id);
                ViewBag.todosPropietarios = propietario.ObtenerTodos();
                return View(inmueble);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            ViewBag.todosPropietarios = propietario.ObtenerTodos();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid && inmueble.Direccion != "")
                {
                    repositorio.Alta(inmueble);
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Inmueble creado con exito";
                    return View();
                }
                else
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();
                        ViewBag.MensajeError = "Uh no, te olvidaste de algo";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.todosPropietarios = propietario.ObtenerTodos();
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            var inmueble = repositorio.ObtenerPorId(id);
            ViewBag.todosPropietarios = propietario.ObtenerTodos();
            return View(inmueble);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inmueble inmueble)
        {
            ViewBag.Id = inmueble.IdInmueble;

            try
            {
                if (ModelState.IsValid)
                {
                    
                    repositorio.Modificacion(inmueble);
                    var inmu = repositorio.ObtenerPorId(inmueble.IdInmueble);
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();


                    ViewBag.Exito = "Cambio guardados con exito";
                        ViewBag.MensajeError = null;
                    return View(inmu);
                }
                else
                    
                    ViewBag.MensajeError = "LLená todos los campos che!";
                
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();

                    var inmue = repositorio.ObtenerPorId(inmueble.IdInmueble);
                    return View(inmue);
            }
            catch
            {

                var inmue = repositorio.ObtenerPorId(inmueble.IdInmueble);

                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                ViewBag.todosPropietarios = propietario.ObtenerTodos();
                return View(inmue);
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var inmueble = repositorio.ObtenerPorId(id);
                return View(inmueble);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inmueble inmueble)
        {
            try
            {
                repositorio.Baja(inmueble.IdInmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult ListContratos(int id)
        {
            try
            {
                var contratos = repositorio.ObtenerContratos(id);
                return View(contratos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }


        public ActionResult ListPropiedades()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListPropiedades(string Nombre, string Apellido)
        {
            try
            {
                if (ModelState.IsValid && !Nombre.Equals("") && !Apellido.Equals(""))
                {
                    var inmuebles = repositorio.ObtenerPorNombrePropietario(Nombre, Apellido);
                    if (inmuebles.Count() == 0)
                    {
                        ViewBag.Error = "No se encontraron resultados";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "";
                        return View(inmuebles);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontraron resultados";
                    ViewBag.misPropietarios = null;
                    var inmuebles = repositorio.ObtenerTodos();
                    return View(inmuebles);
                }
            }
            catch (Exception e)
            {
                ViewBag.StackTrace = e.StackTrace;
                ViewBag.Error = "No se encontraron resultados";
                return View();
            }
        }
    }
}