namespace SIREC.Models
{
    public class MedicoConsultorio
    {
        public int ID { get; set; }

        public int IDUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        public int IDConsultorio { get; set; }
        public Consultorio? Consultorio { get; set; }
    }
}
