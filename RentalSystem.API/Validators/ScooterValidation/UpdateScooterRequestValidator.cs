using FluentValidation;

public class UpdateScooterRequestValidator : AbstractValidator<UpdateScooterRequest>
{
    public UpdateScooterRequestValidator()
    {
        RuleFor(b => b.SerialNumber).IsValidSerialNumber();  // Типа AS7-WXA      
    }
}