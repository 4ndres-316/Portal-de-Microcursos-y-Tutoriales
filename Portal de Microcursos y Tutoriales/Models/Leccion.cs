using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Leccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El curso es obligatorio")]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [StringLength(150, ErrorMessage = "El contenido no puede tener más de 150 caracteres")]
        [Display(Name = "Contenido")]
        public string Contenido { get; set; }

        [Required(ErrorMessage = "La orden es obligatoria")]
        [Display(Name = "Orden")]
        public int Orden { get; set; }


        //Relacion
        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        public ICollection<LeccionCompletada> LeccionesCompletadas { get; set; } = new List<LeccionCompletada>();
    }
}
