using System.ComponentModel.DataAnnotations;

namespace SIREC.Models
{
    public class Consultorio
    {
        [Key]
        public int IDConsultorio { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Ubicacion { get; set; }
        public string HorarioAtencion { get; set; }

        // Relación
        public ICollection<Cita> Citas { get; set; }
    }
}
