using FlightDetailApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightDetailApi.Configurations
{
    public static class DbContextExtensions
    {
        public static void AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            // Registers the application's database context with the dependency injection container
            services.AddDbContext<ApplicationDbContext>(options =>
                // Configures the database context to use SQL Server with the connection string from configuration
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}