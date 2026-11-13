using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal_de_Microcursos_y_Tutoriales.Data;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string UsuarioId { get; set; } = "";

        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [Display(Name = "Fecha de inscripción")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;


        [ForeignKey("UsuarioId")]
        public ApplicationUser? Usuario { get; set; }

        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        public ICollection<LeccionCompletada> LeccionesCompletadas { get; set; } = new List<LeccionCompletada>();
    }
}
