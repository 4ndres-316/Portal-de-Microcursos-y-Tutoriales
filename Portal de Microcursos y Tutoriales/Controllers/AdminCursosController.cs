using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminCursosController : Controller
    {
        private readonly AppDBContext _context;

        public AdminCursosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Lecciones)
                .Include(c => c.Inscripciones)
                .OrderBy(c => c.Titulo)
                .ToListAsync();

            return View(cursos);
        }

        public IActionResult Crear()
        {
            return View(new Curso());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Curso curso)
        {
            if (!ModelState.IsValid)
            {
                return View(curso);
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(curso);
            }

            var existente = await _context.Cursos.FindAsync(id);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Titulo = curso.Titulo;
            existente.Descripcion = curso.Descripcion;
            existente.Nivel = curso.Nivel;
            existente.DuracionHoras = curso.DuracionHoras;
            existente.Publicado = curso.Publicado;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Inscripciones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            if (curso.Inscripciones.Any())
            {
                TempData["Error"] = "No se puede eliminar el curso porque tiene suscriptores inscritos.";
                return RedirectToAction(nameof(Index));
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publicar(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            curso.Publicado = !curso.Publicado;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Inscritos(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Inscripciones)
                .ThenInclude(i => i.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }
    }
}
