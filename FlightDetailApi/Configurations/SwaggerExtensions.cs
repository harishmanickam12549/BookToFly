using Microsoft.OpenApi.Models;

namespace FlightDetailApi.Configurations
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Configures Swagger documentation and security definitions.
        /// </summary>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Adds a Swagger document with title and version
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookToFly",
                    Version = "v1"
                });
                
                // Configures JWT authentication in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' followed by a space and your token."
                });
                
                // Enforces authentication requirement in Swagger UI
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}