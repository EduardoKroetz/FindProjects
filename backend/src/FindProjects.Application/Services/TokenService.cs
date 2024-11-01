using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FindProjects.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new NullReferenceException("Invalid JWT Key");
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}