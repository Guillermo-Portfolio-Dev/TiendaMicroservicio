using Productos.Api.Domain;
using Productos.Api.Infrastructure;

namespace Productos.Api.Features.Products.CreateProduct
{
    public class CreateProductHandler
    {
        private readonly ProductosDbContext _context;

        public CreateProductHandler(ProductosDbContext context)
        {
            _context = context;
        }

        public async Task<Product> ExecuteAsync(CreateProductCommand command)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Nombre = command.Nombre,
                Descripcion = command.Descripcion,
                Precio = command.Precio,
                Stock = command.Stock,
                FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            _context.Productos.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
