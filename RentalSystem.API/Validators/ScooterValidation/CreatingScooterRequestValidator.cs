using FluentValidation;

public class CreatingScooterRequestValidator : AbstractValidator<CreatingScooterRequest>
{
    public CreatingScooterRequestValidator()
    {
        RuleFor(b => b.SerialNumber).IsValidSerialNumber(); // Типа AS7-WXA      
    }
}