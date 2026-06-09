using Microsoft.EntityFrameworkCore;
using Usuarios.Api.Infrastructure;

namespace Usuarios.Api.Features.Users.GetUsers
{
    public class GetUserService
    {
        private readonly UsuariosDbContenxt _context;

        public GetUserService(UsuariosDbContenxt context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> ExeuteAsync()
        {
            return await _context.Usuarios
                .Select(u => new UserDto(u.Id, u.Usuario, u.Nombre_Completo, u.Correo_Electronico))
                .ToListAsync();
        }
    }
}
