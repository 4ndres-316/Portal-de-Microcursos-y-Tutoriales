using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Leccion
    {
        public int Id { get; set; }

        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [Display(Name = "Contenido")]
        public string Contenido { get; set; } = "";

        [Display(Name = "Orden")]
        public int Orden { get; set; }


        //Relacion
        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        public ICollection<LeccionCompletada> LeccionesCompletadas { get; set; } = new List<LeccionCompletada>();
    }
}
