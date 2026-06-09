using Microsoft.EntityFrameworkCore;
using Productos.Api.Domain;

namespace Productos.Api.Infrastructure
{
    public class ProductosDbContext : DbContext
    {
        public ProductosDbContext(DbContextOptions<ProductosDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
