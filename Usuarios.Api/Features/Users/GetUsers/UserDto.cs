namespace Usuarios.Api.Features.Users.GetUsers
{
    public record UserDto(Guid Id, string Usuario, string Nombre_Completo, string Correo_Electronico);
}
