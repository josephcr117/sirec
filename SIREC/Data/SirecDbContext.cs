using Microsoft.EntityFrameworkCore;
using SIREC.Models;

namespace SIREC.Data
{
    public class SirecDbContext : DbContext
    {
        public SirecDbContext(DbContextOptions<SirecDbContext> options) : base(options)
        {
        }

        // DbSets = Tablas en la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<MedicoConsultorio> MedicoConsultorios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de clave compuesta para UsuarioRol (tabla intermedia)
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.IDUsuario, ur.IDRol });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.IDUsuario);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.IDRol);

            modelBuilder.Entity<MedicoConsultorio>()
                .HasKey(mc => mc.ID);

            modelBuilder.Entity<MedicoConsultorio>()
                .HasOne(mc => mc.Usuario)
                .WithMany()
                .HasForeignKey(mc => mc.IDUsuario);

            modelBuilder.Entity<MedicoConsultorio>()
                .HasOne(mc => mc.Consultorio)
                .WithMany()
                .HasForeignKey(mc => mc.IDConsultorio);
        }
    }
}
