using Microsoft.EntityFrameworkCore;
using Usuarios.Api.Domain;

namespace Usuarios.Api.Infrastructure
{
    public class UsuariosDbContenxt : DbContext
    {
        // Constructor corregido para aceptar opciones y crear la base de datos
        public UsuariosDbContenxt(DbContextOptions<UsuariosDbContenxt> options) : base(options)
        {
            // Esta línea es la clave: crea la tabla Usuarios si no existe
            Database.EnsureCreated();
        }

        public DbSet<User> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
