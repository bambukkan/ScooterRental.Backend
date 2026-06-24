using RentalSystem.Core.Models;
public interface IUserService
{
    public Task<List<UserEntity>> Get();
    public Task<UserEntity?> GetById(Guid id);
    public Task<Guid> Add(CreatingUserRequest request);
    public Task Delete(Guid id);
    public Task Update(Guid id,UpdateUserRequest request);
}