using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared;

namespace AuthenticationService.Helper
{
    public class JwtToken
    {
        private readonly IConfiguration _configuration;

        public JwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(
            Guid userId,
            string email,
            string role,
            string? position = null,
            Guid? cinemaId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role , role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(position))
            {
                claims.Add(new Claim("position", position));
            }

            if (cinemaId.HasValue)
            {
                claims.Add(new Claim("cinema_id", cinemaId.Value.ToString()));
            }

            var SECRET_KEY = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:AccessTokenKey"])
            );

            var credential = new SigningCredentials(SECRET_KEY, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1500),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        public string GenerateRefreshToken(string sessionId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, sessionId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var SECRET_KEY = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshTokenKey"])
            );

            var credential = new SigningCredentials(SECRET_KEY, SecurityAlgorithms.HmacSha256);

            var refreshToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public string? VerifyRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshTokenKey"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var sessionId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value;

                return sessionId;
            }
            catch
            {
                return null;
            }
        }
    }
}
