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
    [Authorize]
    public class PagoController : Controller
    {
        private readonly IRepositorio<Pago> repositorio;
        private readonly IRepositorioEmpleado empleadosRepo;

        public PagoController(IRepositorio<Pago> repositorio, IRepositorioEmpleado empleadosRepo)
        {
            this.repositorio = repositorio;
            this.empleadosRepo = empleadosRepo;
        }

        // GET: Pago
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

        // GET: Pago/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var pago = repositorio.ObtenerPorId(id);
                return View(pago);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(pago);
                    TempData["Id"] = pago.IdPago;
                    ViewBag.Exito = "Realizado con éxito";
                    return View();
                }
                else
                    ViewBag.MensajeError = "Parece que cometiste un error";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero seguro hiciste algo mal.";
                return View();
            }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var pago = repositorio.ObtenerPorId(id);
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //que pasa aqui coñooo
                    repositorio.Modificacion(pago);
                    TempData["Id"] = pago.IdPago;
                    ViewBag.Exito = "Realizado con éxito";
                    return View();
                }
                else
                    ViewBag.MensajeError = "Parece que cometiste un error";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero seguro hiciste algo mal.";
                return View();
            }
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var pago = repositorio.ObtenerPorId(id);
                return View(pago);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Pago pago)
        {
            try
            {
                repositorio.Baja(pago.IdPago);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }
    }
}