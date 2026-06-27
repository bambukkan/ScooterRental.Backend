using System.Security.Cryptography;
using RentalSystem.Core.Models;
using RentalSystem.Core.Exceptions;
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
        var user = await _UserRepository.GetById(userId);
        if(user == null)
        {
            throw new KeyNotFoundException($"Пользователь с ID {userId} не найден");
        }

        // тут будут специальные исключения
        if(bookingsOfUser.Count() >= 2)
        {
            throw new UserLimitExceededException();
        }
        var scooterIsBusy = await _BookingRepository.GetByScooterId(request.ScooterId);
        if(scooterIsBusy != null)
        {
            throw new ScooterAlreadyBusyException(request.ScooterId);
        }
        decimal minAmount = 100;
        if(user.Wallet < minAmount)
        {
            throw new InsufficientFundsException(minAmount);
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
    public async Task Update(Guid bookingId, Guid userId)
    {
        var booking = await _BookingRepository.GetById(bookingId);
        var user = await _UserRepository.GetById(userId);

        if (booking == null) throw new EntityNotFoundException($"Аренда с айди {bookingId} не найдена");
        if (booking.EndTime != null) throw new BookingAlreadyFinishedException(bookingId);

        booking.EndTime = DateTime.UtcNow;
        TimeSpan duration = booking.EndTime.Value - booking.StartTime;
        decimal pricePerMinute = 5.69m;
        int minutes = (int)Math.Ceiling(duration.TotalMinutes);
        booking.Price = minutes * pricePerMinute;
        
        var userWallet = user.Wallet - booking.Price;
        await _UserRepository.SaveWalletChanges(user.Id,userWallet);
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