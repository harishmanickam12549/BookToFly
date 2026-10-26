using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace FlightDetailApi.Configurations
{
    public static class JwtAuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"], // Ensure only tokens from this issuer are accepted.

                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"], // Ensure the token is intended for this audience.

                        ValidateIssuerSigningKey = true,
                        // Uses the secret key for validation
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),

                        ValidateLifetime = true // Ensures token has not expired before allowing access.
                    };
                });
        }
    }
}