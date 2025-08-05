using System.ComponentModel.DataAnnotations;

namespace SIREC.Models
{
    public class Rol
    {
        [Key]
        public int IDRol { get; set; }
        public string Nombre { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
