using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InscripcionesController(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Cursos en los que el usuario esta inscrito y su progreso
        public async Task<IActionResult> MisCursos()
        {
            var usuarioId = _userManager.GetUserId(User);

            var inscripciones = await _context.Inscripciones
                .Where(i => i.UsuarioId == usuarioId)
                .Include(i => i.Curso)
                .Include(i => i.LeccionesCompletadas)
                .OrderBy(i => i.FechaInscripcion)
                .ToListAsync();

            var cursosIds = inscripciones.Select(i => i.CursoId).ToList();

            var totales = await _context.Lecciones
                .Where(l => cursosIds.Contains(l.CursoId))
                .GroupBy(l => l.CursoId)
                .Select(g => new { CursoId = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.CursoId, x => x.Total);

            var lista = new List<MisCursosItem>();

            foreach (var inscripcion in inscripciones)
            {
                totales.TryGetValue(inscripcion.CursoId, out var total);

                lista.Add(new MisCursosItem
                {
                    Curso = inscripcion.Curso!,
                    FechaInscripcion = inscripcion.FechaInscripcion,
                    TotalLecciones = total,
                    Completadas = inscripcion.LeccionesCompletadas.Count
                });
            }

            return View(lista);
        }

        // Inscripcion a un curso gratuito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribir(int cursoId)
        {
            var usuarioId = _userManager.GetUserId(User);

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c => c.Id == cursoId && c.Publicado);

            if (curso == null)
            {
                return NotFound();
            }

            var yaInscrito = await _context.Inscripciones
                .AnyAsync(i => i.CursoId == cursoId && i.UsuarioId == usuarioId);

            if (!yaInscrito)
            {
                _context.Inscripciones.Add(new Inscripcion
                {
                    CursoId = cursoId,
                    UsuarioId = usuarioId!,
                    FechaInscripcion = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            // Al inscribirse el usuario pasa a ser suscriptor
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario != null && !await _userManager.IsInRoleAsync(usuario, "Suscriptor"))
            {
                await _userManager.AddToRoleAsync(usuario, "Suscriptor");
            }

            return RedirectToAction(nameof(MisCursos));
        }
    }
}
