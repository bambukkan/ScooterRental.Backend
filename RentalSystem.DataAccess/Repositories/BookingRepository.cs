using Microsoft.EntityFrameworkCore;
using RentalSystem.Core.Models;

public class BookingRepository : IBookingRepository
{
    private readonly RentalSystemDbContext _context;
    public BookingRepository(RentalSystemDbContext context)
    {
        _context = context;
    }
    public async Task<List<BookingEntity>> Get()
    {
        return await _context.Bookings.AsNoTracking().ToListAsync();
    }
    public async Task<List<BookingEntity>> GetWithDetails()
    {
        return await _context.Bookings
        .AsNoTracking()
        .Include(b => b.User)
        .Include(b => b.Scooter)
        .ToListAsync();
    }
    public async Task<List<BookingEntity>> GetListByUserId(Guid Id)
    {
        // Находим все аренды юзера в данный момент
        return await _context.Bookings.AsNoTracking()
        .Where(b => b.UserId == Id && b.EndTime == null)
        .ToListAsync();
    }

    /*Если сервис находит запись, где ScooterId == 111 и EndTime == null, 
    это значит: «Кто-то прямо сейчас катается на этом самокате, его брать нельзя!». Он занят.*/
    public async Task<BookingEntity> GetByScooterId(Guid id)
    {
        return await _context.Bookings.AsNoTracking()
            .FirstOrDefaultAsync(b => b.ScooterId == id && b.EndTime == null);
    }

    public async Task<BookingEntity> GetById(Guid id)
    {
        return await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }
    public async Task Add(BookingEntity booking)
    {
       await  _context.Bookings.AddAsync(booking);
       await _context.SaveChangesAsync();
    }
    public async Task Delete(Guid id)
    {
        await _context.Bookings.Where(b => b.Id == id).ExecuteDeleteAsync();
    }
    public async Task UpdateFinish(BookingEntity booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

}