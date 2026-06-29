namespace RentalSystem.Core.Exceptions;

public class InsufficientFundsException : DomainException
{
    public InsufficientFundsException(decimal minAmount) 
        : base($"Недостаточно средств. Минимальный баланс для старта: {minAmount} руб.") { }
}
