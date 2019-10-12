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
    public class InquilinoController : Controller
    {
        private readonly IRepositorioInquilino repositorio;
        private readonly IRepositorioEmpleado empleadosRepo;

        public InquilinoController(IRepositorioInquilino repositorio, IRepositorioEmpleado empleadosRepo)
        {
            this.repositorio = repositorio;
            this.empleadosRepo = empleadosRepo;
        }

        // GET: Inquilino
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

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var inquilino = repositorio.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {

                if (ModelState.IsValid && inquilino.Nombre != "")
                {
                    repositorio.Alta(inquilino);
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Creado con éxito";
                    return View();
                }
                else
                    ViewBag.MensajeError = "¿Te olvidaste de completar un campo?";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var inquilino = repositorio.ObtenerPorId(id);
            return View(inquilino);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inquilino inquilino)
        {
            ViewBag.Id = inquilino.IdInquilino;

            try
            {
                var inqul = repositorio.ObtenerPorId(inquilino.IdInquilino);

                if (ModelState.IsValid && inquilino.Nombre != "")
                {
                    repositorio.Modificacion(inquilino);
                    ViewBag.Exito = "Editado con exito";
                    return View(inqul);
                }
                else
                    ViewBag.MensajeError = "Dejaste algo sin completar ¿puede ser?";
                    return View(inqul);
            }
            catch (Exception ex)
            {
                var inqul = repositorio.ObtenerPorId(inquilino.IdInquilino);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View(inqul);
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var inquilino = repositorio.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inquilino inquilino)
        {
            try
            {
                repositorio.Baja(inquilino.IdInquilino);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }
        public ActionResult ListInquilinos()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListInquilinos(string nombre, string apellido)
        {
            try
            {
                if (ModelState.IsValid && !nombre.Equals("") && !apellido.Equals(""))
                {
                    var inquilino = repositorio.ObtenerPorNombreApellido(nombre, apellido);

                    if(inquilino.Count() == 0)
                    {
                        ViewBag.Error = "No se encontraron resultados";
                        return View();
                    } 
                    else
                    {
                        ViewBag.Error = "";
                        return View(inquilino);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontraron resultados";
                    return View();
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