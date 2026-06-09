using Orders.Api.Application.Dtos;
using Refit;

namespace Orders.Api.Interfaces
{
    public interface IProductClient
    {
        [Get("/api/products/{id}")]
        Task<ProductDto> GetProductByIdAsync(Guid id);
    }
}
