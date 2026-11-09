using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    [Authorize]
    public class LeccionesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LeccionesController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lleva a la primera leccion del curso
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

        // Contenido de la leccion. Solo para usuarios inscritos en el curso.
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

            // Aunque el usuario escriba la URL, se verifica la inscripcion aqui
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

            ViewData["Lecciones"] = lecciones;

            return View(leccion);
        }
    }
}
