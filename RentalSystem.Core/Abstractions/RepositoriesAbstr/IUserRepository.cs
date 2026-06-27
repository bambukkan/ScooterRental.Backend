using RentalSystem.Core.Models;

public interface IUserRepository
{
    public Task<List<UserEntity>> Get();
    public Task<UserEntity?> GetById(Guid id);
    public Task<UserEntity?> GetByEmail(string email);
    public Task Add(UserEntity User);
    public Task Delete(Guid id);
    public Task Update(Guid id,UpdateUserRequest request);
    public Task SaveChanges(Guid id,UserRole role);

    public Task SaveChangesForRefreshToken(Guid userId,string refreshToken,DateTime refreshTokenExpiryTime);
    public Task<UserEntity?> GetByRefreshToken(string refreshToken);
}