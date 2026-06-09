using Usuarios.Api.Features.Users.GetUsers;
using Usuarios.Api.Infrastructure;

namespace Usuarios.Api.Features.Users.GetUserById
{
    public class GetUserByIdService
    {
        private readonly UsuariosDbContenxt _context;

        public GetUserByIdService(UsuariosDbContenxt context) => _context = context;

        public async Task<UserDto?> ExecuteAsync(Guid id)
        {
            var user = await _context.Usuarios.FindAsync(id);

            if (user == null) return null;

            return new UserDto(user.Id, user.Usuario, user.Nombre_Completo, user.Correo_Electronico);
        }
    }
}
