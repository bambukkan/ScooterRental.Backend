using RentalSystem.Core.Models;

public interface IJwtProvider
{
    string GenerateToken(UserEntity user);
}
