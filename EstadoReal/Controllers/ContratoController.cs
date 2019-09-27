using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorio<Contrato> repositorio;

        public ContratoController(IRepositorio<Contrato> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: Contrato
        public ActionResult Index()
        {
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
                    TempData["Id"] = contrato.IdContrato;
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

        // GET: Contrato/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var contrato = repositorio.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Modificacion(contrato);
                    TempData["Id"] = contrato.IdContrato;
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
    }
}