namespace RentalSystem.Core.Exceptions;

public class ScooterAlreadyBusyException : DomainException
{
    public ScooterAlreadyBusyException(Guid scooterId) 
        : base($"Самокат с ID {scooterId} уже занят!") { }
}
