namespace RentalSystem.Core.Exceptions;

public class UserLimitExceededException : DomainException
{
    public UserLimitExceededException() 
        : base("Превышен лимит одновременных бронирований.") { }
}
