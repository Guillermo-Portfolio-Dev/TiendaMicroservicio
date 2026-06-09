using Microsoft.Extensions.Caching.Distributed;
using Orders.Api.Application.Dtos;
using Orders.Api.Domain;
using Orders.Api.Infrastructure.Persistence;
using Orders.Api.Interfaces;
using Polly.CircuitBreaker;
using Refit;
using System.Text.Json;

namespace Orders.Api.Features.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly MongoDbContext _dbContext;
        private readonly IUserClient _userClient;
        private readonly IProductClient _productClient;
        private readonly IDistributedCache _cache;

        public CreateOrderHandler(MongoDbContext dbContext, IUserClient userClient,
            IProductClient productClient, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _userClient = userClient;
            _productClient = productClient;
            _cache = cache;
        }

        public async Task<string> ExecuteAsync(CreateOrderCommand command)
        {

            // validar que el usuario existe
            try
            {
                var user = await _userClient.GetUserByIdAsync(command.UserId);
            }
            catch (BrokenCircuitException)
            {
                throw new Exception("El servicio de validación de usuarios no está disponible temporalmente. Intente más tarde.");
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("El usuario no existe.");
            }

            // validar productos y obtener precios actualizados
            var orderItems = new List<OrderItem>();
            foreach (var item in command.Items)
            {
                try
                {
                    string cacheKey = $"product_{item.ProductId}";
                    var cachedProduct = await _cache.GetStringAsync(cacheKey);
                    ProductDto product;

                    if (!string.IsNullOrEmpty(cachedProduct))
                    {
                        product = JsonSerializer.Deserialize<ProductDto>(cachedProduct)!;
                    }
                    else
                    {
                        // si no esta en cache, usamos refit
                        product = await _productClient.GetProductByIdAsync(item.ProductId);

                        //guardamos en redis para la proxima vez expira en 10min
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product),options);
                    }
                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Precio, // usamos el precio real de la DB de productos
                    });
                }
                catch (BrokenCircuitException)
                {
                    throw new Exception("El servicio de validacion de productos no esta disponible temporalmente. Intente más tarde.");
                }
                catch (ApiException)
                {
                    throw new Exception($"Error: El producto {item.ProductId} no existe.");
                }
            }

            // crear y guardar el pedido
            var newOrder = new Order
            {
                UserId = command.UserId,
                items = command.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
                TotalAmount = command.Items.Sum(i => i.Quantity * i.UnitPrice),
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await _dbContext.Orders.InsertOneAsync(newOrder);

            return newOrder.Id!; // retornamos el id generado por MongoDB para el nuevo pedido
        }
    }
}
