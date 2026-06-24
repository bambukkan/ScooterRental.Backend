using FluentValidation;

public class CreatingBookingRequestValidator : AbstractValidator<CreatingBookingRequest>
{
    public CreatingBookingRequestValidator()
    {
        RuleFor(b => b.UserId).NotEmpty().WithMessage("Аренда должна иметь пользователь");
        RuleFor(b => b.ScooterId).NotEmpty().WithMessage("Арендовывать нужно самокат");
    }
}