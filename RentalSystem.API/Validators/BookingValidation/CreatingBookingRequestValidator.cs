using FluentValidation;

public class CreatingBookingRequestValidator : AbstractValidator<CreatingBookingRequest>
{
    public CreatingBookingRequestValidator()
    {

        RuleFor(b => b.ScooterId).NotEmpty().WithMessage("Арендовывать нужно самокат");
    }
}