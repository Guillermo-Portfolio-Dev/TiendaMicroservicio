using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Productos.Api.Features.Products.CreateProduct;
using Productos.Api.Features.Products.GetProductById;
using Productos.Api.Features.Products.GetProducts;
using Productos.Api.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// conexión a base de datos

var connectionString = builder.Configuration.GetConnectionString("ProductosConnection");
builder.Services.AddDbContext<ProductosDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// registrar servicios
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetProductsService>();
builder.Services.AddScoped<GetProductByIdService>();

var app = builder.Build();

// Endpoints

app.MapPost("/api/products", async (CreateProductCommand command, CreateProductHandler handler) =>
{
    var product = await handler.ExecuteAsync(command);
    return Results.Created($"/api/products/{product.Id}", product);
})
    .WithName("CreateProduct");

app.MapGet("/api/products", async (GetProductsService service) =>
{
    var products = await service.ExecuteAsync();
    return products;
});

app.MapGet("/api/products/{id}", async (Guid id,GetProductByIdService service) =>
{
    var product = await service.ExecuteAsync(id);
    return product;
});

//app.MapOpenApi();
//app.MapScalarApiReference(options =>
//{
//    options.WithTitle("Productos API - .NET 10")
//           .WithTheme(ScalarTheme.Moon)
//           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Productos API - .NET 10")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
