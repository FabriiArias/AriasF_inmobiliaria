using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    public class ContratoController : Controller
    {
        private readonly ContratoRepo _contratoRepo;
        private readonly InquilinoRepo _inquilinoRepo;
        private readonly InmuebleRepo _inmuebleRepo;

        public ContratoController(ContratoRepo contratoRepo, InquilinoRepo inquilinoRepo, InmuebleRepo inmuebleRepo)
        {
            _contratoRepo = contratoRepo;
            _inquilinoRepo = inquilinoRepo;
            _inmuebleRepo = inmuebleRepo;
        }

        public IActionResult Listar()
        {
             _contratoRepo.ActualizarContratosPendientes();
             _contratoRepo.FinalizarContratos();
            var contratos = _contratoRepo.GetAll();
            return View(contratos);
        }

        public IActionResult Detalle(int id)
        {
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        public IActionResult Crear()
        {
            var inquilinos = _inquilinoRepo.GetAllInquilinos();
            var inmuebles = _inmuebleRepo.GetAllInmuebles();
            ViewBag.Inquilinos = inquilinos;
            ViewBag.Inmuebles = inmuebles;
            return View();
        }

        [HttpPost]
        public IActionResult Insertar(Contrato contrato)
        {

            if (!_inmuebleRepo.EstaDisponible(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin))
                {
                    ModelState.AddModelError("", "El inmueble no está disponible en las fechas seleccionadas.");

                    var contratoOriginal = _contratoRepo.GetById(contrato.IdContrato);
                    return View("Renovar", contratoOriginal);
                }


            if (ModelState.IsValid)
            {
                // Asignar valores predeterminados
                contrato.CreadoPor = 1; // Usuario predeterminado
                contrato.Estado = "Vigente"; // Estado predeterminado

                // Guardar el contrato
                _contratoRepo.InsertContrato(contrato);

                TempData["SuccessMessage"] = "Contrato creado exitosamente.";
                return RedirectToAction("Listar");
            }

            // Si hay errores, volver a cargar los inquilinos e inmuebles
            var inquilinos = _inquilinoRepo.GetAllInquilinos();
            var inmuebles = _inmuebleRepo.GetAllInmuebles();
            ViewBag.Inquilinos = inquilinos;
            ViewBag.Inmuebles = inmuebles;

            return View("Crear", contrato);
        }

        public IActionResult Editar(int id)
        {
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            var inquilinos = _inquilinoRepo.GetAllInquilinos();
            var inmuebles = _inmuebleRepo.GetAllInmuebles();
            ViewBag.Inquilinos = inquilinos;
            ViewBag.Inmuebles = inmuebles;
            return View(contrato);
        }

        public IActionResult FiltrarInmuebles(string tipo, string uso, int ambientes, DateTime fechaInicio, DateTime fechaFin)
        {
            var inmuebles = _inmuebleRepo.GetInmueblesDisponibles(tipo, uso, ambientes, fechaInicio, fechaFin);
            var inquilinos = _inquilinoRepo.GetAllInquilinos();

            ViewBag.Inquilinos = inquilinos;
            ViewBag.Inmuebles = inmuebles;

            return View("Crear");
        }

        public IActionResult AnularVista(int id)
        {
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        [HttpPost]
        public IActionResult Anular(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                contrato.Estado = "Pendiente_anulacion";
                contrato.AnuladoPor = 1; // Reemplazar por el ID del usuario real
                contrato.FechaInicioAnulacion = DateTime.Now;
                contrato.FechaFinAnulacion = contrato.FechaFin;

                _contratoRepo.AnularContrato(contrato);

                TempData["SuccessMessage"] = "Contrato anulado correctamente.";
                return RedirectToAction("Listar");
            }

            return View("AnularVista", contrato);
        }

        public IActionResult Renovar(int id){
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        [HttpGet]
        public JsonResult VerificarDisponibilidad(int inmuebleId, DateTime fechaInicio, DateTime fechaFin)
        {
            var disponible = _inmuebleRepo.EstaDisponible(inmuebleId, fechaInicio, fechaFin);
            return Json(disponible);
        }

        

    }
}