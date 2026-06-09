using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Api.Features.Users.CreateUser;
using Usuarios.Api.Features.Users.GetUserById;
using Usuarios.Api.Features.Users.GetUsers;

namespace Usuarios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CreateUserHandler _handler;
        private readonly GetUserService _getUserService;
        private readonly IValidator<CreateUserCommand> _validator;

        public UsersController(CreateUserHandler handler, IValidator<CreateUserCommand> validator, GetUserService getUserService)
        {
            _handler = handler;
            _validator = validator;
            _getUserService = getUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            // validacion manual
            var validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // ejecucion del handler (CQRS)
            var userId = await _handler.Handle(command);

            // Respuesta estandarizada
            return CreatedAtAction(nameof(Create), new { id = userId }, new { Message = "Usuario creado con éxito", Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _getUserService.ExeuteAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(Guid id, [FromServices] GetUserByIdService service)
        {
            var user = await service.ExecuteAsync(id);

            return user is not null ? Ok(user) : NotFound();
        }
    }
}
