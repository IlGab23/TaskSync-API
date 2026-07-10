using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TaskSync.Application.Interfaces.Security;
using TaskSync.Domain.Entities;

namespace TaskSync.Infrastructure.Security;

public class JwtProvider(IConfiguration config, TimeProvider timeProvider) : IJwtProvider
{
    private readonly IConfiguration _config = config;
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //Aggiungere ruoli in futuro
        };

        var secretKey = _config["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey: Missing in configuration file");
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var expireMinutes = int.Parse(_config["Jwt:ExpireInMinutes"] ?? throw new InvalidOperationException("Jwt:ExpireInMinutes: Missing in configuration file"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = timeProvider.GetUtcNow().AddMinutes(expireMinutes).UtcDateTime,
            Issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer: Missing in configuration file"),
            Audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience: Missing in configuration file"),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }

}
