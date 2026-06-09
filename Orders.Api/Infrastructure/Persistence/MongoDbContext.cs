using MongoDB.Driver;
using Orders.Api.Domain;

namespace Orders.Api.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            // Usamos la ruta jerárquica con ':' para obtener el valor directamente
            var connectionString = configuration["MongoSettings:ConnectionString"];
            var databaseName = configuration["MongoSettings:DatabaseName"];

            // Validación rápida para evitar el error de Null
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No se pudo encontrar la cadena de conexión de MongoDB en appsettings.json");
            }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        // Esta propiedad reemplaza a los DbSet de EF
        public IMongoCollection<Order> Orders=> _database.GetCollection<Order>("Orders");
    }
}
