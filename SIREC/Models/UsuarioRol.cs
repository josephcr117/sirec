using System.ComponentModel.DataAnnotations;

namespace SIREC.Models
{
    public class UsuarioRol
    {
        public int IDUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        public int IDRol { get; set; }
        public Rol Rol { get; set; }
    }
}
