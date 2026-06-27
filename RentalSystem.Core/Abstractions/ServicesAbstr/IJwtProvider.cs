using RentalSystem.Core.Models;

public interface IJwtProvider
{
    public string GenerateToken(UserEntity user);
    public string GenerateRefreshToken();
}
