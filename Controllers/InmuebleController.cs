using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;

namespace InmobiliariaApp.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly InmuebleRepo _inmuebleRepo;
        private readonly PropietarioRepo _propietarioRepo;

        public InmuebleController(InmuebleRepo inmuebleRepo, PropietarioRepo propietarioRepo)
        {
            _inmuebleRepo = inmuebleRepo;
            _propietarioRepo = propietarioRepo;
        }

        public IActionResult Listar()
        {
            var inmuebles = _inmuebleRepo.GetAllInmuebles();
            return View(inmuebles);
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
        public IActionResult Insertar(Inmueble inmueble, IFormFile PortadaFile)
        {
            if (PortadaFile != null && PortadaFile.Length > 0)
            {
                // Guardar el archivo
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(PortadaFile.FileName);
                var rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    PortadaFile.CopyTo(stream);
                }

                // Guardamos la ruta relativa en la propiedad Portada
                inmueble.Portada = "/uploads/" + nombreArchivo;
            }

            _inmuebleRepo.InsertInmueble(inmueble);
            return RedirectToAction("Listar");
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
        public IActionResult Actualizar(Inmueble inmueble, IFormFile PortadaFile)
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

    _inmuebleRepo.UpdateInmueble(inmueble);
    return RedirectToAction("Listar");
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
        public IActionResult Eliminar(int id)
        {
            _inmuebleRepo.DeleteInmueble(id);
            return RedirectToAction("Listar");
        }

    }
}