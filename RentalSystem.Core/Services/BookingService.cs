using System.Security.Cryptography;
using RentalSystem.Core.Models;
public class BookingService : IBookingService
{
    private readonly IBookingRepository _BookingRepository;
    private readonly IUserRepository _UserRepository;

    public BookingService(IBookingRepository BookingRepository,IUserRepository UserRepository)
    {
        _BookingRepository = BookingRepository;
        _UserRepository = UserRepository;
    }
    public async Task<List<BookingEntity>> Get()
    {
        return await _BookingRepository.Get();
    }
    public async Task<List<BookingEntity>> GetWithDetails()
    {
        return await _BookingRepository.GetWithDetails();
    }

    public async Task<Guid> Add(Guid userId,CreatingBookingRequest request)
    {
        var bookingsOfUser = await _BookingRepository.GetListByUserId(userId);
        if(bookingsOfUser.Count() >= 2)
        {
            throw new ArgumentOutOfRangeException(nameof(userId),"Количество заказов пользователя уже равно двум, больше нельзя");
        }
        var scooterIsBusy = await _BookingRepository.GetByScooterId(request.ScooterId);
        if(scooterIsBusy != null)
        {
            throw new ArgumentException(nameof(request.ScooterId),"Самокат уже занят!");
        }
        BookingEntity booking = new BookingEntity()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ScooterId = request.ScooterId,
            StartTime = DateTime.UtcNow
        }; 
        await _BookingRepository.Add(booking);
        return booking.Id;
    }
    public async Task Delete(Guid id)
    {
        await _BookingRepository.Delete(id);
    }
    public async Task Update(Guid id)
    {
        var booking = await _BookingRepository.GetById(id);
        if (booking == null) throw new Exception("Аренда не найдена");
        if (booking.EndTime != null) throw new Exception("Эта аренда уже была завершена");

        booking.EndTime = DateTime.UtcNow;
        TimeSpan duration = booking.EndTime.Value - booking.StartTime;
        decimal pricePerMinute = 5.69m;
        int minutes = (int)Math.Ceiling(duration.TotalMinutes);
        booking.Price = minutes * pricePerMinute;

        await _BookingRepository.UpdateFinish(booking);
    }

    public async Task<List<BookingEntity>> GetListByUserId(Guid userId)
    {
        return await _BookingRepository.GetListByUserId(userId);
    }
    public async Task<BookingEntity> GetByScooterId(Guid id)
    {
        return await _BookingRepository.GetByScooterId(id);
    }

    public async Task<BookingEntity> GetById(Guid id)
    {
        return await _BookingRepository.GetById(id);
    }
}