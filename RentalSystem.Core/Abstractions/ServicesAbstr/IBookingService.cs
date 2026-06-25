using RentalSystem.Core.Models;
public interface IBookingService
{
    public Task<List<BookingEntity>> Get();
    public Task<List<BookingEntity>> GetWithDetails();

    public Task<Guid> Add(Guid userId,CreatingBookingRequest request);
    public Task Delete(Guid id);
    public Task Update(Guid id);

    public Task<List<BookingEntity>> GetListByUserId(Guid id);
    public Task<BookingEntity> GetByScooterId(Guid id);

    public Task<BookingEntity> GetById(Guid id);
}