using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;


public class InquilinoController : Controller
{
    private readonly InquilinoRepo _inquilinoRepo;

    public InquilinoController(InquilinoRepo inquilinoRepo)
    {
        _inquilinoRepo = inquilinoRepo;
    }

    public IActionResult Listar()
    {
        var inquilinos = _inquilinoRepo.GetAllInquilinos();
        return View(inquilinos);
    }

    public IActionResult Detalle(int id)
        {
            Console.WriteLine("id: " + id);
            var propietario = _inquilinoRepo.GetInquilinoByDNI(id);
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

    public IActionResult Insertar(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {
            _inquilinoRepo.CrearInquilino(inquilino);
            return RedirectToAction("Listar");
        }
        return View("Crear", inquilino);
    }

    public IActionResult Editar(int id)
    {
        var inquilino = _inquilinoRepo.GetInquilinoByDNI(id);
        if (inquilino == null)
        {
            return NotFound();
        }
        return View(inquilino);
    }

    public IActionResult Actualizar(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {
            _inquilinoRepo.ActualizarInquilino(inquilino);
            return RedirectToAction("Listar");
        }
        return View("Editar", inquilino);
    }

    public IActionResult Eliminar(int id)
    {
        _inquilinoRepo.DeleteInquilino(id);
        return RedirectToAction("Listar");
    }



}