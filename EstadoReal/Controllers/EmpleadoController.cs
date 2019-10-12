using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    [Authorize(Policy = "Empleado")]
    public class EmpleadoController : Controller
    {
        private readonly IRepositorioEmpleado repositorio;
        private readonly IRepositorioPropietario propietario;

        public EmpleadoController(IRepositorioEmpleado repositorio, IRepositorioPropietario propietario)
        {
            this.repositorio = repositorio;
            this.propietario = propietario;
        }

        // GET: Empleado
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();

            var identity = (ClaimsIdentity)User.Identity;
            var empleadoNombre = identity.Name;
            IEnumerable<Claim> claims = identity.Claims;

            ViewBag.empleado = repositorio.ObtenerPorCorreo(empleadoNombre);

            return View(lista);
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var empleado = repositorio.ObtenerPorId(id);
                return View(empleado);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.StackTrate = e.StackTrace;
                return View();
            }
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado empleado)
        {
            try
            {
                var existeCorreoEmpleado = repositorio.ObtenerPorCorreo(empleado.Correo);

                if (ModelState.IsValid && existeCorreoEmpleado == null)
                {
                    if(propietario.ObtenerPorCorreo(empleado.Correo) != null)
                    {
                        //este correo ya está en uso y este software no permite los mismo correos :(
                        if (TempData.ContainsKey("Id"))
                            ViewBag.Id = TempData["Id"];
                        ViewBag.Mensaje = "Lamentamos informate que no podés elegir este correo. Intenta con otro por favor.";
                        return View();
                    }
                    else
                    {
                        empleado.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: empleado.Clave,
                                                salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                                                prf: KeyDerivationPrf.HMACSHA1,
                                                iterationCount: 1000,
                                                numBytesRequested: 256 / 8));
                        repositorio.Alta(empleado);
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                else
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                ViewBag.Mensaje = "Campo vacío y/o correo no disponible. Intente otro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                ViewBag.Mensaje = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.id = id;
            var empl = repositorio.ObtenerPorId(id);
            return View(empl);
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado empleado)
        {
            try
            {
                if (ModelState.IsValid && empleado.Nombre != "" && empleado.Clave != "")
                {
                    empleado.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: empleado.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Modificacion(empleado);
                    ViewBag.Mensaje = "";
                    ViewBag.MensajeExito = "Usuario editado con exito";
                    return View();
                }
                else
                    ViewBag.Mensaje = "Nuevo complejo sistema detectó que hay campos vacíos";
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                return View();
            }
            catch
            {
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                ViewBag.Mensaje = "No sabemos que pasó pero hiciste algo mal seguro.";
                return View();
            }
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var empl = repositorio.ObtenerPorId(id);
                return View(empl);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // POST: Empleado/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Empleado empleado)
        {
            try
            {
                repositorio.Baja(empleado.IdEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrae = ex.StackTrace;
                return View();
            }
        }

        // GET: Empleado/Delete/5
        public ActionResult ListEmpleados()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListEmpleados(string nombre, string apellido)
        {
            try
            {
                if (ModelState.IsValid && !nombre.Equals("") && !apellido.Equals(""))
                {
                    var empelados = repositorio.ObtenerPorNombreApellido(nombre, apellido);

                    if (empelados.Count() == 0)
                    {
                        ViewBag.Error = "No se encontraron resultados";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "";
                        return View(empelados);
                    }
                }
                else
                {
                    ViewBag.Error = "No se encontraron resultados";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.StackTrace = ex.StackTrace;
                ViewBag.Error = "No se encontraron resultados";
                return View();
            }
        }
    }
}