using FluentValidation;

namespace Usuarios.Api.Features.Users.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("El campo 'Usuario' es obligatorio.")
                .MinimumLength(3).WithMessage("El campo 'Usuario' debe tener al menos 3 caracteres.");
            RuleFor(x=>x.Correo_Electronico)
                .NotEmpty().WithMessage("El campo 'Correo_Electronico' es obligatorio.")
                .EmailAddress().WithMessage("El campo 'Correo_Electronico' debe ser un correo electrónico válido.");
            RuleFor(x=>x.Password)
                .NotEmpty().WithMessage("El campo 'Password' es obligatorio.")
                .MinimumLength(6).WithMessage("El campo 'Password' debe tener al menos 6 caracteres.");
            RuleFor(x=>x.Nombre_Completo)
                .NotEmpty().WithMessage("El campo 'Nombre_Completo' es obligatorio.");
        }
    }
}
