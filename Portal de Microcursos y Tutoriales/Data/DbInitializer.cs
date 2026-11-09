using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal_de_Microcursos_y_Tutoriales.Models;

namespace Portal_de_Microcursos_y_Tutoriales.Data
{
    public static class DbInitializer
    {
        public static async Task InicializarAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDBContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Aplica las migraciones pendientes
            await context.Database.MigrateAsync();

            // Roles
            string[] roles = { "Administrador", "Suscriptor" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // Usuario administrador
            var adminEmail = "admin@portal.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Administrador");
            }

            // Cursos de ejemplo
            if (!context.Cursos.Any())
            {
                var curso1 = new Curso
                {
                    Titulo = "Introducción a C#",
                    Descripcion = "Curso básico para aprender las bases del lenguaje C#.",
                    Nivel = "Básico",
                    DuracionHoras = 4,
                    Publicado = true,
                    Lecciones = new List<Leccion>
                    {
                        new Leccion
                        {
                            Titulo = "Qué es C#",
                            Orden = 1,
                            Contenido = "<p>C# es un lenguaje de programación creado por Microsoft.</p>"
                        },
                        new Leccion
                        {
                            Titulo = "Variables y tipos de datos",
                            Orden = 2,
                            Contenido = "<p>Las variables guardan datos. Algunos tipos son <b>int</b>, <b>string</b> y <b>bool</b>.</p>"
                        },
                        new Leccion
                        {
                            Titulo = "Condicionales",
                            Orden = 3,
                            Contenido = "<p>Usamos <b>if</b> y <b>else</b> para tomar decisiones en el código.</p>"
                        }
                    }
                };

                var curso2 = new Curso
                {
                    Titulo = "HTML y CSS desde cero",
                    Descripcion = "Aprende a maquetar páginas web sencillas con HTML y CSS.",
                    Nivel = "Básico",
                    DuracionHoras = 3,
                    Publicado = true,
                    Lecciones = new List<Leccion>
                    {
                        new Leccion
                        {
                            Titulo = "Estructura de un documento HTML",
                            Orden = 1,
                            Contenido = "<p>Todo documento HTML empieza con <b>&lt;!DOCTYPE html&gt;</b> y la etiqueta <b>html</b>.</p>"
                        },
                        new Leccion
                        {
                            Titulo = "Etiquetas básicas",
                            Orden = 2,
                            Contenido = "<p>Las etiquetas más usadas son <b>h1</b>, <b>p</b>, <b>div</b> y <b>a</b>.</p>"
                        }
                    }
                };

                var curso3 = new Curso
                {
                    Titulo = "SQL Server básico",
                    Descripcion = "Introducción a las consultas y tablas en SQL Server.",
                    Nivel = "Intermedio",
                    DuracionHoras = 6,
                    Publicado = false,
                    Lecciones = new List<Leccion>
                    {
                        new Leccion
                        {
                            Titulo = "Qué es una base de datos relacional",
                            Orden = 1,
                            Contenido = "<p>Una base de datos relacional guarda la información en tablas relacionadas entre sí.</p>"
                        }
                    }
                };

                context.Cursos.AddRange(curso1, curso2, curso3);
                await context.SaveChangesAsync();
            }
        }
    }
}
