using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Leccion> Lecciones { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<LeccionCompletada> LeccionesCompletadas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Un usuario solo se puede inscribir una vez en el mismo curso
            builder.Entity<Inscripcion>()
                .HasIndex(i => new { i.UsuarioId, i.CursoId })
                .IsUnique();

            builder.Entity<Inscripcion>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Inscripcion>()
                .HasOne(i => i.Curso)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Leccion>()
                .HasOne(l => l.Curso)
                .WithMany(c => c.Lecciones)
                .HasForeignKey(l => l.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Una leccion solo se marca como completada una vez
            builder.Entity<LeccionCompletada>()
                .HasIndex(lc => new { lc.InscripcionId, lc.LeccionId })
                .IsUnique();

            builder.Entity<LeccionCompletada>()
                .HasOne(lc => lc.Inscripcion)
                .WithMany(i => i.LeccionesCompletadas)
                .HasForeignKey(lc => lc.InscripcionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LeccionCompletada>()
                .HasOne(lc => lc.Leccion)
                .WithMany(l => l.LeccionesCompletadas)
                .HasForeignKey(lc => lc.LeccionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
