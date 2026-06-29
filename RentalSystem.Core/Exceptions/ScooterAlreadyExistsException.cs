namespace RentalSystem.Core.Exceptions;

public class ScooterAlreadyExistsException : DomainException
{
    public ScooterAlreadyExistsException(string serialNumber)
        : base($"Самокат с таким серийным номером {serialNumber} уже существует!") {}
}