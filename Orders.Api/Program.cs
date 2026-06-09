using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Orders.Api.Features.Orders.CreateOrder;
using Orders.Api.Infrastructure.Persistence;
using Orders.Api.Interfaces;
using Polly;
using Polly.Extensions.Http;
using Refit;
using Scalar.AspNetCore;

// Esta línea le dice a Mongo cómo manejar todos los Guids de la aplicación
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Configuración de MongoDB
var connectionString = builder.Configuration.GetSection("MongoSettings")["ConnectionString"]
                       ?? builder.Configuration["MongoSettings__ConnectionString"];

var databaseName = builder.Configuration.GetSection("MongoSettings")["DatabaseName"]
                   ?? builder.Configuration["MongoSettings__DatabaseName"];

// registramos redis para que este disponible en toda la app
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
    options.InstanceName = "OrdersApi_";
});

// definimos una política: Reintentar 3 veces con una espera exponencial (2s, 4s , 8s)
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // errores 5xx o errores de red
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)); // Si falla 5 veces seguidas, "abre el circuito" y bloquea llamadas por 30 seg.

// cliente para usuarios
builder.Services.AddRefitClient<IUserClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7001"))
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

// cliente para productos
builder.Services.AddRefitClient<IProductClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7002"))
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);


// servicio MongoDB
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

// Registro de infrastructure
builder.Services.AddSingleton<MongoDbContext>();

// Registro de Features
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<CreateOrderValidation>();

var app = builder.Build();

// Minimal Apis

app.MapPost("/api/orders", async (CreateOrderCommand command, CreateOrderHandler handler,
    CreateOrderValidation validator) =>
{
    var validationResult = await validator.ValidateAsync(command);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var result = await handler.ExecuteAsync(command);
    return Results.Created($"/api/orders/{result}",
        new { Message = "Pedido creado exitosamente", OrderId = result });
})
    .WithName("CreateOrder");


app.MapGet("/api/orders", async (MongoDbContext context) =>
{
    var orders = await context.Orders.Find(_ => true).ToListAsync();
    return Results.Ok(orders);
})
    .WithName("GetOrders");


// probar scalar en prod
//app.MapOpenApi();
//app.MapScalarApiReference(config =>
//{
//    config.Title = "Orders API";
//    config.DarkMode = true;
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        config.Title = "Orders API";
        config.DarkMode = true;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
