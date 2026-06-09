namespace Productos.Api.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string FechaCreacion { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
