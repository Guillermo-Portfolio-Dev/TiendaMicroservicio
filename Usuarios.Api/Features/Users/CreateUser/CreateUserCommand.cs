namespace Usuarios.Api.Features.Users.CreateUser
{
    public record CreateUserCommand(string Usuario, string Nombre_Completo, string Correo_Electronico, string Password);
}
