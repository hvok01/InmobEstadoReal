using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;

        public InmuebleController(IRepositorioInmueble repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: Inmueble
        public ActionResult Index()
        {
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
                    TempData["Id"] = inmueble.IdInmueble;
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

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var inmueble = repositorio.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Modificacion(inmueble);
                    TempData["Id"] = inmueble.IdInmueble;
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
    }
}