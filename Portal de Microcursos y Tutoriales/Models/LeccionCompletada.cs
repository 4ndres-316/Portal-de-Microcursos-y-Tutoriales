using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class LeccionCompletada
    {
        public int Id { get; set; }

        [Display(Name = "Inscripción")]
        public int InscripcionId { get; set; }

        [Display(Name = "Lección")]
        public int LeccionId { get; set; }

        [Display(Name = "Fecha de completado")]
        public DateTime FechaCompletado { get; set; } = DateTime.Now;


        [ForeignKey("InscripcionId")]
        public Inscripcion? Inscripcion { get; set; }

        [ForeignKey("LeccionId")]
        public Leccion? Leccion { get; set; }
    }
}
