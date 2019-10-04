using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly IRepositorioEmpleado empleado;

        public PropietarioController(IRepositorioPropietario repositorio, IRepositorioEmpleado empleado)
        {
            this.repositorio = repositorio;
            this.empleado = empleado;
        }

        // GET: Propietario
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var empleadoNombre = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.empleado = empleado.ObtenerPorCorreo(empleadoNombre);

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
                var existeCorreoPropietario = repositorio.ObtenerPorCorreo(propietario.Correo);

                if (ModelState.IsValid && existeCorreoPropietario == null)
                {
                    if (empleado.ObtenerPorCorreo(propietario.Correo) != null)
                    {
                        //el correo ya exite champ
                        return View();
                    }
                    else
                    {
                        propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: propietario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                        repositorio.Alta(propietario);
                        TempData["Id"] = propietario.IdPropietario;
                        ViewBag.Exito = "Propietario registrado con exito";
                        return View();
                    }
                }
                else
                    ViewBag.MensajeError = "No che, sabes que te faltó algo";
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
                    ViewBag.MensajeError = "";
                    ViewBag.Exito = "Propietario editado con exito";
                    return View();
                }
                else
                    ViewBag.MensajeError = "No che, sabes que te faltó algo";
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