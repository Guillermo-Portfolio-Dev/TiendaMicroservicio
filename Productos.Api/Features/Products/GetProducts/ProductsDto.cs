namespace Productos.Api.Features.Products.GetProducts
{
    public record ProductDto(Guid Id, string Nombre, string Descripcion, decimal Precio, int Stock);
}
