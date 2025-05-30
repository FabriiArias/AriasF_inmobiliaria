using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly PagoRepo _pagoRepo;
        private readonly ContratoRepo _contratoRepo;
        private readonly PropietarioRepo _propietarioRepo;

        public PagoController(PagoRepo pagoRepo, ContratoRepo contratoRepo, PropietarioRepo propietarioRepo)
        {
            _pagoRepo = pagoRepo;
            _contratoRepo = contratoRepo;
            _propietarioRepo = propietarioRepo;
        }
    

        public IActionResult Listar(int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var pagos = _pagoRepo.GetAllPagosPaginados(pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(pagos);
        }
        

        public IActionResult ListarPorContrato(int contratoId, int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var pagos = _pagoRepo.GetPagosByContratoPaginado(contratoId, pagina, cantidadPorPagina, out int totalRegistros);
            var contrato = _contratoRepo.GetById(contratoId);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.Contrato = contrato;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.ContratoId = contratoId; // útil para links de paginación

            return View("Listar", pagos);
        }



        public IActionResult Detalle(int id)
        {
            Console.WriteLine("ID Pago: " + id);
            var pago = _pagoRepo.GetPagoById(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        [HttpGet]
        public IActionResult GetContratoInfo(int id)
        {
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
                return NotFound();

            return Json(new
            {
                id = contrato.IdContrato,
                inquilino = contrato.NyAInquilino,   
                direccion = contrato.Direccion,       
                monto = contrato.MontoMensual,        
                fechaInicio = contrato.FechaInicio.ToString("yyyy-MM-dd"),
                fechaFin = contrato.FechaFin.ToString("yyyy-MM-dd"),
                estado = contrato.Estado
            });
        }



       [HttpPost]
        public IActionResult EditarDetalle(string detalle, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna errores de validación
            }
            _pagoRepo.EditarPago(detalle, id); 
            return RedirectToAction("Detalle", new { id });
        }

        [HttpPost]
       public IActionResult RegistrarPago(int id, string detalle)
        {
            var pago = _pagoRepo.GetPagoById(id);
            if (pago == null) return NotFound();

            pago.Detalle = detalle;
            pago.Estado = "Pagado";
            pago.FechaPago = DateTime.Now; // Establecer la fecha de hoy

            _pagoRepo.Abonar(pago);

            return RedirectToAction("Detalle", new { id = pago.IdPago });
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Anular(int id)
        {
            var pago = _pagoRepo.GetPagoById(id);
            if (pago == null) return NotFound();

            pago.Estado = "Anulado";
            _pagoRepo.Anular(pago);

            return RedirectToAction("Detalle", new { id = pago.IdPago });
        }

        [HttpGet]
        public IActionResult Crear(int? id) // id es el id del contrato
        {
            var pago = new Pago();
            if (id.HasValue)
            {
                // Se puede usar si necesitás inicializar algo más adelante
                ViewBag.ContratoId = id.Value;
            }
            return View(pago);
        }



        [HttpPost]
        public IActionResult InsertPago(Pago pago)
        {
            // Obtener el id del usuario logueado desde los claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario");
            if (userIdClaim == null)
            {
                // No está autenticado o no tiene el claim, manejar error o redirigir
                TempData["Error"] = "No estás autenticado.";
                return RedirectToAction("Crear"); // O la vista que corresponda
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            // Asignar el id del usuario logueado al pago
            pago.CreadoPor = idUsuario;

            // Validar el modelo antes de insertar
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Errores: " + string.Join("; ", errores);
                return View("Crear", pago);
            }

            _pagoRepo.InsertPago(pago);

            TempData["Mensaje"] = "Pago registrado correctamente.";
            return RedirectToAction("Listar");
        }



        

    }
}