using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    [Authorize]
    public class LeccionesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeccionesController(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Curso(int cursoId)
        {
            var leccion = await _context.Lecciones
                .Where(l => l.CursoId == cursoId)
                .OrderBy(l => l.Orden)
                .FirstOrDefaultAsync();

            if (leccion == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Ver), new { id = leccion.Id });
        }

        public async Task<IActionResult> Ver(int id)
        {
            var leccion = await _context.Lecciones
                .Include(l => l.Curso)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leccion == null)
            {
                return NotFound();
            }

            var usuarioId = _userManager.GetUserId(User);

            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.CursoId == leccion.CursoId && i.UsuarioId == usuarioId);

            if (inscripcion == null)
            {
                return Forbid();
            }

            var lecciones = await _context.Lecciones
                .Where(l => l.CursoId == leccion.CursoId)
                .OrderBy(l => l.Orden)
                .ToListAsync();

            var completadas = await _context.LeccionesCompletadas
                .Where(lc => lc.InscripcionId == inscripcion.Id)
                .Select(lc => lc.LeccionId)
                .ToListAsync();

            ViewData["Lecciones"] = lecciones;
            ViewData["Completadas"] = completadas;

            return View(leccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarCompletada(int id)
        {
            var leccion = await _context.Lecciones
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leccion == null)
            {
                return NotFound();
            }

            var usuarioId = _userManager.GetUserId(User);

            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.CursoId == leccion.CursoId && i.UsuarioId == usuarioId);

            if (inscripcion == null)
            {
                return Forbid();
            }

            var yaCompletada = await _context.LeccionesCompletadas
                .AnyAsync(lc => lc.InscripcionId == inscripcion.Id && lc.LeccionId == id);

            if (!yaCompletada)
            {
                _context.LeccionesCompletadas.Add(new LeccionCompletada
                {
                    InscripcionId = inscripcion.Id,
                    LeccionId = id,
                    FechaCompletado = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Ver), new { id });
        }
    }
}
