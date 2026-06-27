namespace RentalSystem.Core.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException() : base("Неверный email или пароль.") { }
}

public class ScooterAlreadyBusyException : DomainException
{
    public ScooterAlreadyBusyException(Guid scooterId) 
        : base($"Самокат с ID {scooterId} уже занят!") { }
}

public class UserLimitExceededException : DomainException
{
    public UserLimitExceededException() 
        : base("Превышен лимит одновременных бронирований.") { }
}

public class InsufficientFundsException : DomainException
{
    public InsufficientFundsException(decimal minAmount) 
        : base($"Недостаточно средств. Минимальный баланс для старта: {minAmount} руб.") { }
}

public class BookingAlreadyFinishedException : DomainException
{
    public BookingAlreadyFinishedException(Guid bookingId)
        : base($"Аренда {bookingId} уже завершилась!") {}
}
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message)
        : base(message) {}
}
public class ScooterAlreadyExistsException : DomainException
{
    public ScooterAlreadyExistsException(string serialNumber)
        : base($"Самокат с таким серийным номером {serialNumber} уже занят") {}
}