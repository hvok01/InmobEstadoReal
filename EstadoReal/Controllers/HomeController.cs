using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EstadoReal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstadoReal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioPropietario propietarios;
        private readonly IRepositorioEmpleado empleadosRepo;
        private readonly DataContext contexto;

        public HomeController(IRepositorioPropietario propietarios, IRepositorioEmpleado empleadosRepo, DataContext contexto)
        {
            this.propietarios = propietarios;
            this.empleadosRepo = empleadosRepo;
            this.contexto = contexto;
        }

        // GET: Home
        public ActionResult Index()
        {
            /*
            ViewBag.Titulo = "Página de Inicio";
            List<string> clientes = contexto.Propietarios.Select(x => x.Nombre + " " + x.Apellido).ToList();
            return View(clientes);
             */

            return View();
        }

        // GET: Home/Login
        public ActionResult Login()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if(identity.Name == null)
            {
                return View();
            } else
            {
                return RedirectToAction("Index", "Empleado");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView loginView)
        {
                try
                {

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: loginView.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    var e = empleadosRepo.ObtenerPorCorreo(loginView.Usuario);

                    if (e == null || e.Clave != hashed)
                    {
                        ViewBag.Mensaje = "Datos inválidos";
                        return View();
                    }
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, e.Correo),
                    new Claim("FullName", e.Nombre + " " + e.Apellido),
                    new Claim(ClaimTypes.Role, "Empleado"),
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.
                        AllowRefresh = true,
                    };

                    TempData["Id"] = e.IdEmpleado;

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return RedirectToAction("Index", "Empleado");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                    return View();
                }
            
        }

        public ActionResult Registrar()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.Name == null)
            {
                return View();
            } else
            {
                return RedirectToAction("Index", "Empleado");
            }
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Empleado empleado)
        {
            try
            {
                var existeCorreoEmpleado = empleadosRepo.ObtenerPorCorreo(empleado.Correo);

                if (ModelState.IsValid && existeCorreoEmpleado == null)
                {
                    if (propietarios.ObtenerPorCorreo(empleado.Correo) != null)
                    {
                        //este correo ya está en uso y este software no permite los mismo correos :(
                        ViewBag.MensajeError = "Este correo ya fue registrado :(";
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
                        empleadosRepo.Alta(empleado);
                        TempData["Id"] = empleado.IdEmpleado;
                        ViewBag.Exito = "Registrado con exito.";
                        return View();
                    }

                }
                else
                    ViewBag.MensajeError = "Campos vacíos u/o el correo ya está en uso.";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }


            // GET: Home/Login
            public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Seguro()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        [Authorize(Policy = "Empleado")]
        public ActionResult Admin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        public ActionResult Restringido()
        {
            return View();
        }

    }
}