using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IRepositorio<Propietario> repositorio;

        public PropietarioController(IRepositorio<Propietario> repositorio)
        {
            this.repositorio = repositorio;
        }

        // GET: Propietario
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        // GET: Propietario/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                TempData["Nombre"] = propietario.Nombre;
                if(ModelState.IsValid)
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Alta(propietario);
                    TempData["Id"] = propietario.IdPropietario;
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

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var prop = repositorio.ObtenerPorId(id);
            return View(prop);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Propietario propietario)
        {
            try
            {
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
                    TempData["Id"] = propietario.IdPropietario;
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

        // GET: Propietario/Delete/5
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
    }
}