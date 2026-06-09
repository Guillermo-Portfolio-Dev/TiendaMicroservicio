using Mapster;
using Usuarios.Api.Domain;
using Usuarios.Api.Infrastructure;

namespace Usuarios.Api.Features.Users.CreateUser
{
    public class CreateUserHandler
    {
        private readonly UsuariosDbContenxt _context;

        public CreateUserHandler(UsuariosDbContenxt context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateUserCommand command)
        {
            // 1. Mapeo de command a entidad
            var user = command.Adapt<User>();

            user.Id = Guid.NewGuid();
            user.Fecha_Creacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

    }
}
