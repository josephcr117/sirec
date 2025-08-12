namespace SIREC.Models
{
    public class UsuarioUpdateDto
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Correo { get; set; } = "";
        public string? Contraseña { get; set; } 
        public string Cedula { get; set; } = "";
        public string Telefono { get; set; } = "";
        public DateTime? FechaNacimiento { get; set; } // opcional
        public int? IdRol { get; set; } // opcional, si se envía, se actualiza
    }
}
