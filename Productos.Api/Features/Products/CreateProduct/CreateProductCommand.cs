namespace Productos.Api.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Nombre,string Descripcion,decimal Precio,int Stock);
}
