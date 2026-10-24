using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightDetailApi.Services
{
    public class JWTTokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        // Constructor to initialize JWT configuration values
        public JWTTokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JWT:SecretKey"];
            _issuer = configuration["JWT:Issuer"];
            _audience = configuration["JWT:Audience"];
        }

        // Token Structure: HEADER.PAYLOAD.SIGNATURE

        /// <summary>
        /// Generates a JWT token for authentication and authorization.
        /// </summary>
        /// <param name="username">The username of the authenticated user.</param>
        /// <param name="role">The role assigned to the user.</param>
        /// <returns>Returns a JWT token as a string.</returns>
        public string GenerateJWTToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                // Unique identifier for the token to prevent reuse.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Generate a security key using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            // Convert the token to a string format and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
