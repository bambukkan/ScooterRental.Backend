using Microsoft.EntityFrameworkCore;
using RentalSystem.Core.Models;

public class ScooterRepository : IScooterRepository
{
    private readonly RentalSystemDbContext _context;
    public ScooterRepository(RentalSystemDbContext context)
    {
        _context = context;
    }
    public async Task<List<ScooterEntity>> Get()
    {
        return await _context.Scooters.AsNoTracking().ToListAsync();
    }
    public async Task<List<ScooterEntity>> GetWithDetails()
    {
        return await _context.Scooters
            .AsNoTracking()
            .Include(s => s.Bookings)
            .ToListAsync();
    }
    
    public async Task<ScooterEntity> GetBySerialNumber(string serNum)
    {
        return await _context.Scooters
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SerialNumber == serNum);
    }
    public async Task Add(ScooterEntity Scooter)
    {
        await _context.Scooters.AddAsync(Scooter);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(Guid id)
    {
        await _context.Scooters.Where(s => s.Id == id).ExecuteDeleteAsync();
    }
    public async Task Update(Guid id,UpdateScooterRequest request)
    {
        await _context.Scooters.Where(s => s.Id == id).ExecuteUpdateAsync(s =>
        s.SetProperty(s => s.SerialNumber,request.SerialNumber));
    }

}