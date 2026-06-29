namespace RentalSystem.Core.Exceptions;

public class BookingAlreadyFinishedException : DomainException
{
    public BookingAlreadyFinishedException(Guid bookingId)
        : base($"Аренда {bookingId} уже завершилась!") {}
}
