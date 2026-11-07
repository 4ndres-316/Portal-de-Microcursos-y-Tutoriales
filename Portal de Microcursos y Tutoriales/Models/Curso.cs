using System.ComponentModel.DataAnnotations;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El nivel es obligatorio")]
        [Display(Name = "Nivel")]
        public string Nivel { get; set; } = "Básico";

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Display(Name = "Duración (horas)")]
        public float DuracionHoras { get; set; }

        [Required(ErrorMessage = "La publicación es obligatoria")]
        [Display(Name = "Publicado")]
        public bool Publicado { get; set; } = false;


        //Relacion
        public ICollection<Leccion> Lecciones { get; set; } = new List<Leccion>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}
