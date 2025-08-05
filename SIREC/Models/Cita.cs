using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIREC.Models
{
    public class Cita
    {
        [Key]
        public int IDCita { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan Hora { get; set; }

        public string Estado { get; set; } = "Solicitada"; // Estado por defecto, Solicitada, Confirmada, Cancelada, Completada

        public string Observaciones { get; set; }

        public int IDUsuario { get; set; }

        [ForeignKey("IDUsuario")]
        public Usuario? Usuario { get; set; }

        public int IDConsultorio { get; set; }

        [ForeignKey("IDConsultorio")]
        public Consultorio? Consultorio { get; set; }
    }
}
