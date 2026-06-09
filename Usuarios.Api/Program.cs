using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Usuarios.Api.Features.Users.CreateUser;
using Usuarios.Api.Features.Users.GetUserById;
using Usuarios.Api.Features.Users.GetUsers;
using Usuarios.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<UsuariosDbContenxt>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Registro del handler
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<GetUserService>();
builder.Services.AddScoped<GetUserByIdService>();

// Registro de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

var app = builder.Build();

//app.MapOpenApi();
//app.MapScalarApiReference(options =>
//{
//    options.WithTitle("Usuarios API")
//           .WithTheme(ScalarTheme.Moon)
//           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
//});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Configura la interfaz de Scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Usuarios API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Migraciones automaticas

// --- BLOQUE DE MIGRACIONES AUTOMÁTICAS ---
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<UsuariosDbContenxt>();
//    // 'EnsureCreated' es la clave: Crea la tabla "Usuarios" si no existe en Postgres
//    context.Database.EnsureCreated();
//    Console.WriteLine("--> Base de datos y tablas creadas/validadas en Docker.");
//}

app.Run();
