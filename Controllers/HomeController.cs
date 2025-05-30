using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AriasF_inmobiliaria.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace AriasF_inmobiliaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsuarioRepo _usuarioRepo;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, UsuarioRepo usuarioRepo, IConfiguration config)
        {
            _logger = logger;
            _usuarioRepo = usuarioRepo;
            _config = config;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string usuario, string clave)
        {
            var user = _usuarioRepo.GetUsuarioByEmail(usuario);

            if (user != null && user.Activo)
            {
                // Hashear la contraseña ingresada por el usuario
                string hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                // Comparar el hash ingresado con el guardado
                if (user.Password == hashedInput)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Rol),
                        new Claim("Nombre", user.Nombre),
                        new Claim("Apellido", user.Apellido),
                        new Claim("IdUsuario", user.IdUsuario.ToString())  
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index");
                }
            }

            ViewBag.Mensaje = "Credenciales incorrectas o usuario inactivo";
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("AccessDenied");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
