using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly ContratoRepo _contratoRepo;
        private readonly InquilinoRepo _inquilinoRepo;
        private readonly InmuebleRepo _inmuebleRepo;
        private readonly PagoRepo _pagoRepo;

        public ContratoController(ContratoRepo contratoRepo, InquilinoRepo inquilinoRepo, InmuebleRepo inmuebleRepo, PagoRepo pagoRepo)
        {
            _contratoRepo = contratoRepo;
            _inquilinoRepo = inquilinoRepo;
            _inmuebleRepo = inmuebleRepo;
            _pagoRepo = pagoRepo;
        }

        public IActionResult Listar()
        {
             _contratoRepo.ActualizarContratosPendientes();
             _contratoRepo.FinalizarContratos();
            var contratos = _contratoRepo.GetAll();
            return View(contratos);
        }

        // get all contratos select 2

        public JsonResult GetAllContratosSelect2(string term)
        {
            var contratos = _contratoRepo.GetAll()
                .Where(c => !string.IsNullOrEmpty(c.NyAInquilino) && c.NyAInquilino.ToLower().Contains(term.ToLower()))
                .Select(c => new
                {
                    id = c.IdContrato,
                    text = $"{c.NyAInquilino} - {c.Direccion} ({c.NyAUser})"
                });
            return Json(contratos);
        }


        // filtrar por inmueble

        [HttpGet]
        public IActionResult FiltrarPorInmueble(int inmuebleId, int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            if (pagina < 1) pagina = 1;

            var contratos = _contratoRepo.GetContratosPorInmueble(inmuebleId, pagina, cantidadPorPagina, out int totalRegistros);

            if (contratos == null || contratos.Count == 0)
            {
                ViewBag.Mensaje = "No se encontraron contratos para el inmueble seleccionado.";
                return View("Listar", new List<Contrato>());
            }

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.InmuebleId = inmuebleId;

            return View("Listar", contratos);
        }


        // filtrar por fechas vigentes

        /*[HttpGet]
        public IActionResult FiltrarPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var contratos = _contratoRepo.GetContratosPorFecha(fechaInicio, fechaFin);
            if (contratos == null || contratos.Count == 0)
            {
                ViewBag.Mensaje = "No se encontraron contratos en el rango de fechas seleccionado.";
                return View("Listar", new List<Contrato>());
            }
            return View("Listar", contratos);
        }*/

        [HttpGet]
        public IActionResult FiltrarPorFechas(DateTime fechaInicio, DateTime fechaFin, int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            if (pagina < 1) pagina = 1;

            var contratos = _contratoRepo.GetContratosPorFecha(fechaInicio, fechaFin, pagina, cantidadPorPagina, out int totalRegistros);

            if (contratos == null || contratos.Count == 0)
            {
                ViewBag.Mensaje = "No se encontraron contratos en el rango de fechas seleccionado.";
                return View("Listar", new List<Contrato>());
            }

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.FechaInicio = fechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.ToString("yyyy-MM-dd");

            return View("Listar", contratos);
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
        TempData["Error"] = "El inmueble no está disponible en las fechas seleccionadas.";
        
        // Cargar listas para la vista
        ViewBag.Inquilinos = _inquilinoRepo.GetAllInquilinos();
        ViewBag.Inmuebles = _inmuebleRepo.GetAllInmuebles();

        return View("Crear", contrato);
    }

    if (ModelState.IsValid)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario");
        int idUsuario = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

        contrato.CreadoPor = idUsuario;
        contrato.Estado = "Vigente";

        _contratoRepo.InsertContrato(contrato);

        TempData["Mensaje"] = "Contrato creado exitosamente.";

        // Cargar listas para la vista
        ViewBag.Inquilinos = _inquilinoRepo.GetAllInquilinos();
        ViewBag.Inmuebles = _inmuebleRepo.GetAllInmuebles();

        return View("Crear", contrato);
    }

    TempData["Error"] = "Hay errores en el formulario.";

    // Cargar listas para la vista
    ViewBag.Inquilinos = _inquilinoRepo.GetAllInquilinos();
    ViewBag.Inmuebles = _inmuebleRepo.GetAllInmuebles();

    return View("Crear", contrato);
}



        public IActionResult Editar(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

        [Authorize(Roles = "administrador")]
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
        [Authorize(Roles = "administrador")]
        public IActionResult Anular(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                // Obtener el idUsuario logueado desde los claims
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario");
                int idUsuario = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                contrato.Estado = "Pendiente_anulacion";
                contrato.AnuladoPor = idUsuario;
                contrato.FechaInicioAnulacion = DateTime.Now;
                contrato.FechaFinAnulacion = contrato.FechaFin;

                _contratoRepo.AnularContrato(contrato);

                TempData["SuccessMessage"] = "Contrato anulado correctamente.";
                GenerarMulta(contrato.IdContrato, contrato.FechaFin);
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

        [HttpPost]
        public IActionResult GenerarMulta(int idContrato, DateTime fechaAAnular)
        {
            var contrato = _contratoRepo.GetById(idContrato);
            if (contrato == null)
            {
                return NotFound();
            }

            var hoy = DateTime.Today;
            var fechaInicio = contrato.FechaInicio;
            var fechaFin = contrato.FechaFin;
            var totalDias = (fechaFin - fechaInicio).TotalDays;
            var diasTranscurridos = (fechaAAnular - fechaInicio).TotalDays;

            Console.WriteLine($"Fecha Anulacion: {fechaAAnular}");
            Console.WriteLine($"Dias Totales: {totalDias}");
            Console.WriteLine($"Dias Transcurridos: {diasTranscurridos}");

            Console.WriteLine($"Total Dias: {totalDias}");
            Console.WriteLine($"Dias Transcurridos: {diasTranscurridos}");


            if (diasTranscurridos < 0)
            {
                TempData["ErrorMessage"] = "El contrato aún no ha comenzado.";
                return RedirectToAction("Detalle", new { id = idContrato });
            }

            int mesesMulta = diasTranscurridos >= totalDias / 2 ? 1 : 2;

            Console.WriteLine($"Meses de multa: {mesesMulta}");

            var inmueble = _inmuebleRepo.GetInmuebleXID(contrato.IdInmueble);
            double importeMulta = inmueble.Precio * mesesMulta;

            var pagoMulta = new Pago
            {
                IdContrato = idContrato,
                Importe = importeMulta,
                Detalle = "Multa por rescisión anticipada",
                Estado = "Pendiente" 
            };

            _pagoRepo.InsertPago(pagoMulta);

            TempData["SuccessMessage"] = $"Se generó una multa de {importeMulta:C} correspondiente a {mesesMulta} mes(es) de alquiler.";
            return RedirectToAction("Detalle", new { id = idContrato });
        }


        // metodos para los filtrados

        [HttpGet]
        public IActionResult GetContratosPorInquilino(int idInquilino)
        {
            var contratos = _contratoRepo.GetContratosPorInquilino(idInquilino);

            if (contratos == null || contratos.Count == 0)
                return Json(new { success = false, message = "No se encontraron contratos." });

            return Json(new { success = true, data = contratos });
        }
        
        [HttpGet]
        public JsonResult GetContratoById(int id)
        {
            var contrato = _contratoRepo.GetById(id);
            if (contrato == null)
            {
                return Json(null);
            }

            return Json(new
            {
                id = contrato.IdContrato,
                text = $"{contrato.NyAInquilino} - {contrato.Direccion} ({contrato.NyAUser})",
                montoMensual = contrato.MontoMensual
            });
        }

       [HttpGet]
public IActionResult FiltrarPorFinalizacion(int? dias, int? diasPersonalizados, int pagina = 1)
{
    const int cantidadPorPagina = 10;
    if (pagina < 1) pagina = 1;

    int diasFiltro = dias ?? diasPersonalizados ?? 0;
    var contratos = new List<Contrato>();

    if (diasFiltro > 0)
    {
        contratos = _contratoRepo.GetContratosPorDiasHastaFinalizar(diasFiltro, pagina, cantidadPorPagina, out int totalRegistros);

        int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.DiasSeleccionados = diasFiltro;
    }

    ViewBag.Dias = dias;
    ViewBag.DiasPersonalizados = diasPersonalizados;

    return View("Listar", contratos);
}


    }
}