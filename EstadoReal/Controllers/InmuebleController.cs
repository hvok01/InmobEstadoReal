﻿using System;
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
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioEmpleado empleadosRepo;
        private readonly IRepositorioPropietario propietario;

        public InmuebleController(IRepositorioInmueble repositorio, IRepositorioEmpleado empleadosRepo, IRepositorioPropietario propietario)
        {
            this.repositorio = repositorio;
            this.empleadosRepo = empleadosRepo;
            this.propietario = propietario;
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
                    if (TempData.ContainsKey("Exito"))
                    ViewBag.Exito = "Inmueble creado con exito";
                    return View();
                }
                else
                    if (TempData.ContainsKey("MensajeError"))
                    ViewBag.MensajeError = "Uh no, te olvidaste de algo";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                if (TempData.ContainsKey("MensajeError"))
                ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var inmueble = repositorio.ObtenerPorId(id);
            ViewBag.todosPropietarios = propietario.ObtenerTodos();
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
                    var inmu = repositorio.ObtenerPorId(inmueble.IdInmueble);
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();
                    if (TempData.ContainsKey("Exito"))
                        ViewBag.Exito = "Cambio guardados con exito";
                    return View(inmu);
                }
                else
                    if (TempData.ContainsKey("Exito"))
                        ViewBag.MensajeError = "LLená todos los campos che!";
                
                    ViewBag.todosPropietarios = propietario.ObtenerTodos();
                    return View();
            }
            catch
            {
                ViewBag.todosPropietarios = propietario.ObtenerTodos();
                if (TempData.ContainsKey("Exito"))
                    ViewBag.MensajeError = "No sabemos que pasó pero hiciste algo mal seguro.";
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