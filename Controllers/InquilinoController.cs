using Microsoft.AspNetCore.Mvc;
using InmobiliariaApp.Models;
using InmobiliariaApp.Repositories;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class InquilinoController : Controller
{
    
    private readonly InquilinoRepo _inquilinoRepo;

    public InquilinoController(InquilinoRepo inquilinoRepo)
    {
        _inquilinoRepo = inquilinoRepo;
    }

    public IActionResult Listar(int pagina = 1)
    {
        const int cantidadPorPagina = 10;

        var inquilinos = _inquilinoRepo.GetAllInquilinosPaginados(pagina, cantidadPorPagina, out int totalRegistros);

        int totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidadPorPagina);

        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;

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
            TempData["Mensaje"] = "El inquilino se ha creado correctamente";
            return View("Crear", inquilino);
        }else{   
            TempData["Error"] = "Error al crear el inquilino. Por favor, revise los datos ingresados.";
            return View("Crear", inquilino);
        }
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
            TempData["Mensaje"] = "El inquilino se ha actualizado correctamente";
            _inquilinoRepo.ActualizarInquilino(inquilino);
            return View("Editar", inquilino);
        }else {
            TempData["Error"] = "Error al actualizar el inquilino. Por favor, revise los datos ingresados.";
            return View("Editar", inquilino);
        }
    }

    [Authorize(Roles = "administrador")]
    public IActionResult Eliminar(int id)
    {
        _inquilinoRepo.DeleteInquilino(id);
        return RedirectToAction("Listar");
    }


    // metodos para filtrar inquilinos por nombre, apellido, dni, etc.

    [HttpGet]
    public JsonResult GetAllSelect2(string term)
    {
        var inquilinos = _inquilinoRepo.GetAllInquilinos(); // o BuscarPorNombre(term)
        
        var resultado = inquilinos
            .Where(i => string.IsNullOrEmpty(term) || 
                        (i.Nombre + " " + i.Apellido).ToLower().Contains(term.ToLower()))
            .Select(i => new
            {
                id = i.Id,
                text = i.Nombre + " " + i.Apellido
            });

        return Json(resultado);
    }

    




}