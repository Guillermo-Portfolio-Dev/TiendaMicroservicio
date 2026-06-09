namespace Usuarios.Api.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombre_Completo { get; set; } = string.Empty;
        public string Correo_Electronico { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Fecha_Creacion { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
