using Microsoft.IdentityModel.Tokens;
using SuperStoreEcommerceAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperStoreEcommerceAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) => _config = config;

        public (string token, DateTime expiresUtc) CreateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key missing");
            var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 120;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Email ?? user.UserName ?? string.Empty)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expires);
        }
    }
}
