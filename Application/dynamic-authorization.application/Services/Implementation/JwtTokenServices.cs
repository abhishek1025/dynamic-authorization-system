using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dynamic_authorization.application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Microsoft.Extensions.Configuration;

namespace dynamic_authorization.application.Services;

public class JwtTokenService: IJwtTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken(string userId, string email)
    {
        var secretKey = _configuration.GetConnectionString("SecretKey");
        var issuer = _configuration.GetConnectionString("Issuer");
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            secretKey)
        );
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sid,userId),
            new Claim(JwtRegisteredClaimNames.Email,email),
        };
        var token = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

     
    public string GetUserIdFromToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext == null)
        {
            System.Diagnostics.Debug.WriteLine("HttpContext is null in JwtTokenService");
            return null;  // Avoid NullReferenceException
        }
        
        var sidClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c 
            => c.Type == JwtRegisteredClaimNames.Sid);
        
        return sidClaim.Value;
    }
}