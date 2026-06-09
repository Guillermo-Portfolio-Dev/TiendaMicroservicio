using Microsoft.EntityFrameworkCore;
using Productos.Api.Infrastructure;

namespace Productos.Api.Features.Products.GetProducts
{
    public class GetProductsService
    {
        private readonly ProductosDbContext _context;

        public GetProductsService(ProductosDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> ExecuteAsync()
        {
            return await _context.Productos
           .Select(p => new ProductDto(p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock))
           .ToListAsync();
        }
    }
}
