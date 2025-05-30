using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;


namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepo _usuarioRepo;
        private readonly IConfiguration _config;


        public UsuarioController(UsuarioRepo usuarioRepo, IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _config = config;
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Listar()
        {
            var usuarios = _usuarioRepo.GetAllUsuarios();
            return View(usuarios);
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var usuario = _usuarioRepo.GetUsuarioById(id);
            if (usuario == null)
            {
                return NotFound(); // o RedirectToAction("Index") si preferís
            }

            return View(usuario);
        }


        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult Insertar(Usuario usuario, IFormFile AvatarFile)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", usuario); // Retorna a la vista de creación con los errores
            }
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatar");
                Directory.CreateDirectory(carpetaDestino); // Asegura que exista

                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                var rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    AvatarFile.CopyTo(stream);
                }

                usuario.Avatar = "/uploads/avatar/" + nombreArchivo;
            }
            else
            {
                usuario.Avatar = "/uploads/avatar/noavatar.jpg";
            }

            usuario.Activo = true; // Activado por defecto
            usuario.Password = Hash(usuario); // Hashear la contraseña
            _usuarioRepo.InsertUsuario(usuario);
            return RedirectToAction("Listar");
        }


        
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioRepo.GetUsuarioById(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Actualizar(Usuario usuario, IFormFile AvatarFile)
        {

            var usuarioExistente = _usuarioRepo.GetUsuarioById(usuario.IdUsuario);

            if (usuarioExistente == null)
                return NotFound();

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatar");
                Directory.CreateDirectory(carpetaDestino);

                // Eliminar el avatar anterior (si no es el default)
                if (!string.IsNullOrEmpty(usuarioExistente.Avatar) && usuarioExistente.Avatar != "/uploads/avatar/noavatar.jpg")
                {
                    var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuarioExistente.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(rutaAnterior))
                        System.IO.File.Delete(rutaAnterior);
                }

                // Guardar el nuevo avatar
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                var rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    AvatarFile.CopyTo(stream);
                }

                usuario.Avatar = "/uploads/avatar/" + nombreArchivo;
            }
            else
            {
                // Si no se sube nuevo avatar, conservar el anterior
                usuario.Avatar = usuarioExistente.Avatar;
            }
            // Si la contraseña no se ha modificado, conservar la anterior
            if (string.IsNullOrEmpty(usuario.Password) || usuario.Password == usuarioExistente.Password)
            {
                usuario.Password = usuarioExistente.Password;
            }
            else
            {
                // Si se ha modificado, hashearla
                usuario.Password = Hash(usuario);
            }
            _usuarioRepo.UpdateUsuario(usuario);
            int idUsuarioLogueado = int.Parse(User.FindFirstValue("IdUsuario"));
            return RedirectToAction("Detalle", new { id = idUsuarioLogueado });
        }


        [HttpPost]
        public IActionResult BorrarAvatar(int id)
        {
            var usuario = _usuarioRepo.GetUsuarioById(id);
            if (usuario == null)
                return NotFound();

            if (!string.IsNullOrEmpty(usuario.Avatar))
            {
                var rutaAvatar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.Avatar.TrimStart('/'));
                if (System.IO.File.Exists(rutaAvatar))
                {
                    System.IO.File.Delete(rutaAvatar);
                }

                usuario.Avatar = null;
                _usuarioRepo.UpdateUsuario(usuario);
            }

            return RedirectToAction("Editar", new { id });
        }


        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult Eliminar(int id)
        {
            var usuario = _usuarioRepo.GetUsuarioById(id);
            if (usuario != null && !string.IsNullOrEmpty(usuario.Avatar) && usuario.Avatar != "/uploads/avatar/noavatar.jpg")
            {
                var rutaAvatar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.Avatar.TrimStart('/'));
                if (System.IO.File.Exists(rutaAvatar))
                    System.IO.File.Delete(rutaAvatar);
            }

            _usuarioRepo.DeleteUsuario(id);
            return RedirectToAction("Listar");
        }

        private string Hash(Usuario usuario)
        {
            string hasheo = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Password,
                    salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                        numBytesRequested: 256 / 8));
            return hasheo;
        }
    }
}


// login normal sin hasheo por si me mando alguna cagada
/*
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string usuario, string clave)
        {
            var user = _usuarioRepo.GetUsuarioByEmail(usuario);

            if (user != null && user.Password == clave && user.Activo)
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

            ViewBag.Mensaje = "Credenciales incorrectas o usuario inactivo";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }*/