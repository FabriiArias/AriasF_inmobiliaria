using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly InmuebleRepo _inmuebleRepo;
        private readonly PropietarioRepo _propietarioRepo;

        public InmuebleController(InmuebleRepo inmuebleRepo, PropietarioRepo propietarioRepo)
        {
            _inmuebleRepo = inmuebleRepo;
            _propietarioRepo = propietarioRepo;
        }

        /*
        public IActionResult Listar()
        {
            var inmuebles = _inmuebleRepo.GetAllInmuebles();
            return View(inmuebles);
        }*/

        public IActionResult Listar(int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var inmuebles = _inmuebleRepo.GetAllInmueblesPaginados(pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(inmuebles);
        }

        // ------------------- select 2 -----------------

       [HttpGet]
        public JsonResult GetAllInmueblesSelect2(string term)
        {
            var inmuebles = _inmuebleRepo.GetAllInmuebles();

            var resultado = inmuebles
                .Where(i => string.IsNullOrEmpty(term) ||
                            i.Direccion.ToLower().Contains(term.ToLower()))
                .Select(i => new
                {
                    id = i.IdInmueble,
                    text = i.Direccion + " (" + i.NombrePropietario + " " + i.ApellidoPropietario + ")"
                });

            return Json(resultado);
        }



        // ---------------------- filtros ----------------------
        // ----------------- ppor propietario -----------------
        /*
        public IActionResult BuscarPorPropietario(string nombrePropietario)
        {
            var resultado = _inmuebleRepo.BuscarPorPropietario(nombrePropietario);
            return View("Listar", resultado);
        }*/

        // por propietario paginado

        public IActionResult BuscarPorPropietarioPaginado(string nombrePropietario, int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var inmuebles = _inmuebleRepo.BuscarPorPropietarioPaginado(nombrePropietario, pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View("Listar", inmuebles);
        }

        // ----------------- por estado disponible -----------------

        /*public IActionResult BuscarDisponibles()
        {
            var resultado = _inmuebleRepo.ObtenerDisponibles();
            return View("Listar", resultado);
        }*/

        // por estado disponible paginado

        public IActionResult BuscarDisponiblesPaginado(int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var inmuebles = _inmuebleRepo.ObtenerDisponiblesPaginado(pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View("Listar", inmuebles);
        }

        // por fecha no contratada


        /*public IActionResult BuscarPorFecha(DateTime fecha)
        {
            var resultado = _inmuebleRepo.ObtenerDisponiblesPorFecha(fecha);
            return View("Listar", resultado);
        }*/

        // por fecha no contratada paginado

        public IActionResult BuscarPorFechaPaginado(DateTime fecha, int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var inmuebles = _inmuebleRepo.ObtenerDisponiblesPorFechaPaginado(fecha, pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View("Listar", inmuebles);
        }

        public IActionResult Detalle(int id)
        {
            var inmueble = _inmuebleRepo.GetInmuebleXID(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        public IActionResult Crear()
        {
            var propietarios = _propietarioRepo.GetAllPropietarios();
            ViewBag.Propietarios = propietarios;
            return View();
        }


        [HttpPost]
        public IActionResult Insertar(Inmueble inmueble, IFormFile? PortadaFile)
        {
            if (PortadaFile != null && PortadaFile.Length > 0)
            {
                // Ruta donde se guardarán las imágenes
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Nombre único para el archivo
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(PortadaFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardar el archivo en el disco
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    PortadaFile.CopyTo(stream);
                }

                // Guardar la ruta relativa en el modelo
                inmueble.Portada = "/uploads/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                _inmuebleRepo.InsertInmueble(inmueble);
                TempData["Mensaje"] = "El inmueble se ha creado correctamente.";
                // Recargar propietarios para la vista
                ViewBag.Propietarios = _propietarioRepo.GetAllPropietarios();
                return View("Crear", inmueble);
            }
            else
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Errores: " + string.Join("; ", errores);
                
                // Recargar propietarios para la vista
                ViewBag.Propietarios = _propietarioRepo.GetAllPropietarios();
                return View("Crear", inmueble);
            }
        }



        public IActionResult Editar(int id)
        {
            var inmueble = _inmuebleRepo.GetInmuebleXID(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            var propietarios = _propietarioRepo.GetAllPropietarios();
            ViewBag.Propietarios = propietarios;
            return View(inmueble);
        }
        
        [HttpPost]
        public IActionResult Actualizar(Inmueble inmueble, IFormFile? PortadaFile)
        {
            if (PortadaFile != null && PortadaFile.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Eliminar la portada anterior si existe
                if (!string.IsNullOrEmpty(inmueble.Portada))
                {
                    var rutaPortadaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", inmueble.Portada.TrimStart('/'));
                    if (System.IO.File.Exists(rutaPortadaAnterior))
                    {
                        System.IO.File.Delete(rutaPortadaAnterior);
                    }
                }

                // Guardar la nueva portada
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(PortadaFile.FileName);
                var rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

                
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    PortadaFile.CopyTo(stream);
                }

                // Actualizar la nueva portada
                inmueble.Portada = "/uploads/" + nombreArchivo;
            }
            
                if (ModelState.IsValid)
                {
                    _inmuebleRepo.UpdateInmueble(inmueble);
                    TempData["Mensaje"] = "El inmueble se ha creado correctamente.";
                    // Recargar propietarios para la vista
                    ViewBag.Propietarios = _propietarioRepo.GetAllPropietarios();
                    return View("Crear", inmueble);
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    TempData["Error"] = "Errores: " + string.Join("; ", errores);
                    
                    // Recargar propietarios para la vista
                    ViewBag.Propietarios = _propietarioRepo.GetAllPropietarios();
                    return View("Crear", inmueble);
                }
            }

            [HttpPost]
            public IActionResult? BorrarPortada(int id)
            {
                var inmueble = _inmuebleRepo.GetInmuebleXID(id);
                if (inmueble == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(inmueble.Portada))
                {
                    var rutaPortada = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", inmueble.Portada.TrimStart('/'));
                    if (System.IO.File.Exists(rutaPortada))
                    {
                        System.IO.File.Delete(rutaPortada);
                    }

                    // Limpiar el campo Portada del inmueble
                    inmueble.Portada = null;
                    _inmuebleRepo.UpdateInmueble(inmueble);
                }

                return RedirectToAction("Listar");
            }

        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult Eliminar(int id)
        {
            _inmuebleRepo.DeleteInmueble(id);
            return RedirectToAction("Listar");
        }

    }
}