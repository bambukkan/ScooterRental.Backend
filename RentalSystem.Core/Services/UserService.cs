using RentalSystem.Core.Models;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    public UserService(IUserRepository userRepository,IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    public async Task<List<UserEntity>> Get()
    {
        return await _userRepository.Get();
    }
    public async Task<UserEntity?> GetById(Guid id)
    {
        return await _userRepository.GetById(id);
    }
    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }
    public async Task<string> Login(LoginUserRequest request)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Failed to login");
        }
        var token = _jwtProvider.GenerateToken(user); 
        return token;
    }
    public async Task<Guid> Add(CreatingUserRequest request)
    {
        var hashedPassword = _passwordHasher.Generate(request.Password);
        UserEntity user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            PasswordHash = hashedPassword,
            Email = request.Email
        };        
        await _userRepository.Add(user);
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
    public async Task ChangeRole(Guid userId,UserRole role)
    {
        await _userRepository.SaveChanges(userId, role);
    }
}