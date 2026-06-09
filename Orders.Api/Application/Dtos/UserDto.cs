namespace Orders.Api.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombre_Completo { get; set; } = string.Empty;
        public string Correo_Electronico { get; set; } = string.Empty;
    }
}
