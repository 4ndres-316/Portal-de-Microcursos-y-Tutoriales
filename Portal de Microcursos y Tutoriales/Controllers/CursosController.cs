using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Data;

namespace Portal_de_Microcursos_y_Tutoriales.Controllers
{
    public class CursosController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CursosController(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cursos = await _context.Cursos
                .Where(c => c.Publicado)
                .Include(c => c.Lecciones)
                .OrderBy(c => c.Titulo)
                .ToListAsync();

            return View(cursos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Lecciones)
                .FirstOrDefaultAsync(c => c.Id == id && c.Publicado);

            if (curso == null)
            {
                return NotFound();
            }

            var usuarioId = _userManager.GetUserId(User);

            ViewData["Inscrito"] = usuarioId != null &&
                await _context.Inscripciones.AnyAsync(i => i.CursoId == id && i.UsuarioId == usuarioId);

            return View(curso);
        }
    }
}
