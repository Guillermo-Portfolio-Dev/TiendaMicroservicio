using Productos.Api.Features.Products.GetProducts;
using Productos.Api.Infrastructure;

namespace Productos.Api.Features.Products.GetProductById
{
    public class GetProductByIdService
    {
        private readonly ProductosDbContext _context;

        public GetProductByIdService(ProductosDbContext context) => _context = context;

        public async Task<ProductDto?> ExecuteAsync(Guid id)
        {
            var product = await _context.Productos.FindAsync(id);
            if (product == null) return null;

            return new ProductDto(product.Id, product.Nombre, product.Descripcion, product.Precio, product.Stock);
        }
    }
}
