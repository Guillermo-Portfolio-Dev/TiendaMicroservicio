using FluentValidation;

namespace Orders.Api.Features.Orders.CreateOrder
{
    public class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidation()
        {
            RuleFor(x=>x.UserId).NotEmpty().WithMessage("El ID de usuario es obligatorio");
            RuleFor(x => x.Items).NotEmpty().WithMessage("El pedido debe tener al menos un producto");

            // Validamos cada item dentro de la lista
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i=>i.ProductId).NotEmpty().WithMessage("El ID del producto es obligatorio");
                item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero");
                item.RuleFor(i=> i.UnitPrice).GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero");
            });
        }
    }
}
