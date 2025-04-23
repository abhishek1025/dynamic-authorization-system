namespace dynamic_authorization.application.Services.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(string userId, string email);
    public string GetUserIdFromToken();
}