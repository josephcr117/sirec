using System.ComponentModel.DataAnnotations;

namespace SIREC.Models
{
    public class Usuario
    {
        [Key]
        public int IDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }

        // Relaciones
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
