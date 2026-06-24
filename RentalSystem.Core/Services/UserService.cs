using RentalSystem.Core.Models;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;


    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;

    }
    public async Task<List<UserEntity>> Get()
    {
        return await _userRepository.Get();
    }
    public async Task<UserEntity?> GetById(Guid id)
    {
        return await _userRepository.GetById(id);
    }

    public async Task<Guid> Add(CreatingUserRequest request)
    {
        UserEntity user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname
        };        await _userRepository.Add(user);
        return user.Id;
    }

    public async Task Delete(Guid id)
    {
        await _userRepository.Delete(id);
    }
    public async Task Update(Guid id,UpdateUserRequest request)
    {
        await _userRepository.Update(id,request);
    }
}