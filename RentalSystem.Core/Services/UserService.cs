using RentalSystem.Core.Models;
using RentalSystem.Core.Exceptions;
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
    public async Task<(string, string)> Login(LoginUserRequest request)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }
        var token = _jwtProvider.GenerateToken(user); 
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        DateTime refreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
        await _userRepository.SaveChangesForRefreshToken(user.Id,refreshToken,refreshTokenExpiryTime);

        return (token,refreshToken);
    }
    public async Task<(string,string,Guid)> Refresh(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshToken(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Unauthorized / Token expired"); 
        }
        var newToken = _jwtProvider.GenerateToken(user);

        var newRefreshToken = _jwtProvider.GenerateRefreshToken();
        DateTime refreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

        await _userRepository.SaveChangesForRefreshToken(user.Id,newRefreshToken,refreshTokenExpiryTime);

        return (newToken,newRefreshToken,user.Id);
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
            Email = request.Email,
            Wallet = request.Wallet
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