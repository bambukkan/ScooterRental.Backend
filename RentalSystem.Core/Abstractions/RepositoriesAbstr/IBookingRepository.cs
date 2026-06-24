

using RentalSystem.Core.Models;



public interface IBookingRepository
{
    public Task<List<BookingEntity>> Get();
    public Task<List<BookingEntity>> GetWithDetails();
    public Task<List<BookingEntity>> GetListByUserId(Guid id);
    public Task<BookingEntity> GetByScooterId(Guid id);

    public Task<BookingEntity> GetById(Guid id);
    public Task Add(BookingEntity booking);
    public Task Delete(Guid id);
    public Task UpdateFinish(BookingEntity booking);

}