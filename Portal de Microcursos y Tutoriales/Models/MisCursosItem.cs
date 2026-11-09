namespace Portal_de_Microcursos_y_Tutoriales.Models
{
    public class MisCursosItem
    {
        public Curso Curso { get; set; } = new Curso();
        public DateTime FechaInscripcion { get; set; }
        public int TotalLecciones { get; set; }
        public int Completadas { get; set; }

        public int Porcentaje
        {
            get
            {
                if (TotalLecciones == 0)
                {
                    return 0;
                }

                return (Completadas * 100) / TotalLecciones;
            }
        }
    }
}
