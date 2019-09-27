using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IRepositorio<Inquilino> repositorio;

        public InquilinoController(IRepositorio<Inquilino> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: Inquilino
        public ActionResult Index()
        {
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
                    TempData["Id"] = inquilino.IdInquilino;
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
            try
            {
                if (ModelState.IsValid && inquilino.Nombre != "")
                {
                    repositorio.Modificacion(inquilino);
                    TempData["Id"] = inquilino.IdInquilino;
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
    }
}