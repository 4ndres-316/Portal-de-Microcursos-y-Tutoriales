using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminLeccionesController : Controller
    {
        private readonly AppDBContext _context;

        public AdminLeccionesController(AppDBContext context)
        {
            _context = context;
        }

        // Lecciones del curso, ordenadas
        public async Task<IActionResult> Index(int cursoId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Lecciones)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null)
            {
                return NotFound();
            }

            ViewData["Curso"] = curso;

            return View(curso.Lecciones.OrderBy(l => l.Orden).ToList());
        }

        public IActionResult Crear(int cursoId)
        {
            return View(new Leccion { CursoId = cursoId, Orden = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Leccion leccion)
        {
            if (!ModelState.IsValid)
            {
                return View(leccion);
            }

            _context.Lecciones.Add(leccion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { cursoId = leccion.CursoId });
        }

        public async Task<IActionResult> Editar(int id)
        {
            var leccion = await _context.Lecciones.FindAsync(id);

            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Leccion leccion)
        {
            if (id != leccion.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(leccion);
            }

            var existente = await _context.Lecciones.FindAsync(id);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Titulo = leccion.Titulo;
            existente.Contenido = leccion.Contenido;
            existente.Orden = leccion.Orden;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { cursoId = existente.CursoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var leccion = await _context.Lecciones.FindAsync(id);

            if (leccion == null)
            {
                return NotFound();
            }

            var cursoId = leccion.CursoId;

            _context.Lecciones.Remove(leccion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { cursoId });
        }
    }
}
