using APP_ALUMNOS_CONSUME_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace APP_ALUMNOS_CONSUME_API.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly AlumnosService _service;
        public AlumnosController(AlumnosService service)
        {
            _service = service;
        }

        // GET: AlumnosController
        public async Task<IActionResult> Index()
        {
            var alumnos = await _service.Consultar();
            return View(alumnos);
        }

        // GET: AlumnosController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var alumnos = await _service.Consultar(id);
            if(id == null || alumnos == null)
            {
                return NotFound();
            }
            return View(alumnos);
        }

        // GET: AlumnosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlumnosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre")] AlumnosView alumnos)
        {
            if (ModelState.IsValid)
            {
                var edo = await _service.Agregar(alumnos);
                return RedirectToAction(nameof(Index));
            }
            return View(alumnos);
        }

        // GET: AlumnosController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {   
            if(id==null)
            {
                return NotFound();
            }
            var alumnos = await _service.Consultar(id);
            if(alumnos == null)
            {
                return NotFound();
            }
            return View(alumnos);
        }

        // POST: AlumnosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,[Bind("id,nombre")] AlumnosView alumnos)
        {
            if(id != alumnos.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    await _service.Actualizar(alumnos);
                    return RedirectToAction(nameof(Index));
                }
                catch(DBConcurrencyException)   
                {
                    if(await _service.Consultar(alumnos.Id)==null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(alumnos);
        }

        // GET: AlumnosController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var alumnos = await _service.Consultar(id);
            return View(alumnos);
        }

        // POST: AlumnosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
