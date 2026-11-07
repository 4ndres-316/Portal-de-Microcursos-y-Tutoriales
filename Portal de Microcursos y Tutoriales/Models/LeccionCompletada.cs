using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class LeccionCompletada
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La inscripción es obligatoria")]
        [Display(Name = "Inscripción")]
        public int InscripcionId { get; set; }

        [Required(ErrorMessage = "La lección es obligatoria")]
        [Display(Name = "Lección")]
        public int LeccionId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "Debe ser una fecha válida")]
        [Display(Name = "Fecha de Completado")]
        public DateTime FechaCompletado { get; set; }


        //Relacion
        [ForeignKey("InscripcionId")]
        public Inscripcion? Inscripcion { get; set; }

        [ForeignKey("LeccionId")]
        public Leccion? Leccion { get; set; }
    }
}
