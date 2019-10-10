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
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioEmpleado empleadoRepo;
        private readonly IRepositorioInquilino inquilino;
        private readonly IRepositorioInmueble inmueble;
        private readonly IRepositorioPago pago;

        public ContratoController(IRepositorioContrato repositorio, IRepositorioEmpleado empleadoRepo, IRepositorioInquilino inquilino, IRepositorioInmueble inmueble, IRepositorioPago pago)
        {
            this.repositorio = repositorio;
            this.empleadoRepo = empleadoRepo;
            this.inquilino = inquilino;
            this.inmueble = inmueble;
            this.pago = pago;
        }

        // GET: Contrato
        public ActionResult Index()
        {

            var identity = (ClaimsIdentity)User.Identity;
            var empleadoNombre = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.empleado = empleadoRepo.ObtenerPorCorreo(empleadoNombre);

            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var contrato = repositorio.ObtenerPorId(id);
                ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                return View(contrato);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
            ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(contrato);
                    ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                    ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Contrato creado con exito";
                    return View();
                }
                else
                    ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                    ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                    ViewBag.MensajeError = "Uh no, mandaste uno o mas campos vacíos.";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro";
                return View();
            }
        }

        // GET: Contrato/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
            ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
            var contrato = repositorio.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contrato contrato)
        {
            ViewBag.id = contrato.IdContrato;
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Modificacion(contrato);
                    var con = repositorio.ObtenerPorId(contrato.IdContrato);
                    ViewBag.MensajeError = null;
                    ViewBag.Exito = "Contrato editado con exito";
                    ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                    ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                    return View(con);
                }
                else
                    ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                    ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                    var cont = repositorio.ObtenerPorId(contrato.IdContrato);
                    ViewBag.MensajeError = "Uh no, mandaste uno o mas campos vacíos.";
                    return View(cont);
            }
            catch (Exception ex)
            {
                ViewBag.InquilinoTodos = inquilino.ObtenerTodos();
                ViewBag.InmuebleTodos = inmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                var cont = repositorio.ObtenerPorId(contrato.IdContrato);
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro";
                return View(cont);
            }
        }

        // GET: Contrato/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var contrato = repositorio.ObtenerPorId(id);
                return View(contrato);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Contrato contrato)
        {
            try
            {
                repositorio.Baja(contrato.IdContrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }
        // GET: Contrato/Buscar/5
        public ActionResult DateFilter()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DateFilter(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ResultadosContratos = repositorio.BuscarEntreFechas(contrato.InicioContrato, contrato.FinContrato);
                    return View();
                } else
                {
                    ViewBag.ResultadosContratos = null;
                    return View();
                }
                    
            }
            catch (Exception ex)
            {
                ViewBag.ResultadosContratos = null;
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // GET: Contrato/Buscar/5
        public ActionResult ListPagos(int id)
        {
            ViewBag.MisPagos = pago.ObtenerPagosPorContrato(id);
            return View();
        }
    }
}