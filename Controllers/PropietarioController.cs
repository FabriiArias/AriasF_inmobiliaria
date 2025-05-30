using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly PropietarioRepo _propietarioRepo;

        public PropietarioController(PropietarioRepo propietarioRepo)
        {
            _propietarioRepo = propietarioRepo;
        }
        // por aca antes de todo hay que verificar al usuario logueado
        // si esta log y si es admin o empleado
        public IActionResult Listar(int pagina = 1)
        {
            const int cantidadPorPagina = 10;

            var propietarios = _propietarioRepo.GetAllPropietariosPaginados(pagina, cantidadPorPagina, out int totalRegistros);

            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(propietarios);
        }


        public IActionResult Detalle(int dni)
        {
            Console.WriteLine("DNI: " + dni);
            var propietario = _propietarioRepo.GetPropietarioByDNI(dni);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        public IActionResult Crear()
        {
            return View();
        }

        public IActionResult Insertar(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                TempData["Mensaje"] = "Propietario creado correctamente";
                _propietarioRepo.InsertPropietario(propietario);
                return View("Crear", propietario);
            }else{
                TempData["ErrorMessage"] = "Error al crear el propietario. Por favor, revise los datos ingresados.";
                return View("Crear", propietario);
            }
            return View("Crear", propietario);
        }

        public IActionResult Editar(int dni)
        {
            var propietario = _propietarioRepo.GetPropietarioByDNI(dni);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        public IActionResult Actualizar(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                _propietarioRepo.UpdatePropietario(propietario);
                TempData["Mensaje"] = "Propietario actualizado correctamente";
                return View("Editar", propietario);
            }else{
                TempData["ErrorMessage"] = "Error al actualizar el propietario. Por favor, revise los datos ingresados.";
                return View("Editar", propietario);
            }
            
        }
        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult Eliminar(int dni)
        {
            _propietarioRepo.deletePropietario(dni);
            return RedirectToAction("Listar");
        }


    }
}