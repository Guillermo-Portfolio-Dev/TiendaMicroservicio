namespace Orders.Api.Features.Orders.CreateOrder
{
    public record CreateOrderCommand(Guid UserId,List<OrderItemCommand> Items);
    public record OrderItemCommand(Guid ProductId, int Quantity, decimal UnitPrice);
}
