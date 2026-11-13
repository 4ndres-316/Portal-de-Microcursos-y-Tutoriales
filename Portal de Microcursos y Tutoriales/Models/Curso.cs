using System.ComponentModel.DataAnnotations;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El nivel es obligatorio")]
        [StringLength(30)]
        [Display(Name = "Nivel")]
        public string Nivel { get; set; } = "Básico";

        [Range(1, 100, ErrorMessage = "La duración debe estar entre 1 y 100 horas")]
        [Display(Name = "Duración (horas)")]
        public int DuracionHoras { get; set; }

        [Display(Name = "Publicado")]
        public bool Publicado { get; set; }


        public ICollection<Leccion> Lecciones { get; set; } = new List<Leccion>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}
