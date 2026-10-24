using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightDetailApi.Configurations
{
    public static class CROSextensions
    {
        /// <summary>
        /// Adds a default CORS policy to allow any origin, method, and header.
        /// </summary>
        /// <param name="services">The service collection to configure CORS for.</param>
        public static void AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.AllowAnyOrigin()  
                        .AllowAnyMethod()  
                        .AllowAnyHeader()); 
            });
        }
    }
}
