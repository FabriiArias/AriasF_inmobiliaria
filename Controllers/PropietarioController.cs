using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaApp.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly PropietarioRepo _propietarioRepo;

        public PropietarioController(PropietarioRepo propietarioRepo)
        {
            _propietarioRepo = propietarioRepo;
        }
        // por aca antes de todo hay que verificar al usuario logueado
        // si esta log y si es admin o empleado
        public IActionResult Listar()
        {
            //return View();
            var propietarios = _propietarioRepo.GetAllPropietarios();
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
                TempData["SuccessMessage"] = "Regresando a la lista de propietarios";
                _propietarioRepo.InsertPropietario(propietario);
                //return RedirectToAction("Listar");
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
                TempData["SuccessMessage"] = "Propietario actualizado correctamente";
                return RedirectToAction("Listar");
            }
            return View("Editar", propietario);
        }
        [HttpPost]
        public IActionResult Eliminar(int dni)
        {
            _propietarioRepo.deletePropietario(dni);
            return RedirectToAction("Listar");
        }


    }
}