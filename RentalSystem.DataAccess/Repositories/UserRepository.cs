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

}