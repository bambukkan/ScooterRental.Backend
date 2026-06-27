using Microsoft.EntityFrameworkCore;
using RentalSystem.Core.Models;

public class UserRepository : IUserRepository
{
    private readonly RentalSystemDbContext _context;
    public UserRepository(RentalSystemDbContext context)
    {
        _context = context;
    }
    public async Task<List<UserEntity>> Get()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }
    public async Task<UserEntity?> GetById(Guid userId)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task Add(UserEntity User)
    {
        await _context.Users.AddAsync(User);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(Guid id)
    {
        await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
    }
    public async Task Update(Guid id,UpdateUserRequest request)
    {
        await _context.Users.Where(u => u.Id == id).
            ExecuteUpdateAsync(
            s => s
            .SetProperty(u => u.Name,request.Name)
            .SetProperty(u => u.Surname,request.Surname)
        );
    }
    public async Task SaveChanges(Guid id,UserRole role)
    {
        await _context.Users.Where(u => u.Id == id).
            ExecuteUpdateAsync(
            s => s
            .SetProperty(u => u.Role,role)
        );
    }
    public async Task SaveChangesForRefreshToken(Guid userId,string refreshToken,DateTime refreshTokenExpiryTime)
    {
        await _context.Users.Where(u => u.Id == userId).
            ExecuteUpdateAsync(
            s => s
            .SetProperty(u => u.RefreshToken,refreshToken)
            .SetProperty(u => u.RefreshTokenExpiryTime,refreshTokenExpiryTime)
        );
    }
    public async Task<UserEntity?> GetByRefreshToken(string refreshToken)
    {
        return await _context.Users.FirstOrDefaultAsync(
            u => u.RefreshToken == refreshToken
        );
    }

    public async Task SaveWalletChanges(Guid id,decimal wallet)
    {
        await _context.Users.Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => 
            s.SetProperty(u => u.Wallet,wallet)
            );
    }
}