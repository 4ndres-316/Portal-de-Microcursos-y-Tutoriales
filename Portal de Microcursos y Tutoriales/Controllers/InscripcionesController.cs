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
        private readonly UserManager<IdentityUser> _userManager;

        public InscripcionesController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Cursos en los que el usuario esta inscrito
        public async Task<IActionResult> MisCursos()
        {
            var usuarioId = _userManager.GetUserId(User);

            var inscripciones = await _context.Inscripciones
                .Where(i => i.UsuarioId == usuarioId)
                .Include(i => i.Curso)
                .OrderBy(i => i.FechaInscripcion)
                .ToListAsync();

            return View(inscripciones);
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
