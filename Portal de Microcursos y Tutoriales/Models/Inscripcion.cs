using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "El curso es obligatorio")]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "La fecha de inscripción es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida")]
        [Display(Name = "Fecha de Inscripción")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;


        //Relacion
        /*[ForeignKey("UsuarioId")]
        public IdentityUser Usuario { get; set; }*/

        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        public ICollection<LeccionCompletada> LeccionesCompletadas { get; set; } = new List<LeccionCompletada>();
    }
}
