using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    public class PagoController : Controller
    {
        private readonly IRepositorio<Pago> repositorio;

        public PagoController(IRepositorio<Pago> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: Pago
        public ActionResult Index()
        {
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
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
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
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View();
            }
            catch
            {
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