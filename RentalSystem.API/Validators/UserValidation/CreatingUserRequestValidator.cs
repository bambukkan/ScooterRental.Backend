using FluentValidation;

public class CreatingUserRequestValidator : AbstractValidator<CreatingUserRequest>
{
    public CreatingUserRequestValidator()
    {
        RuleFor(b => b.Name).IsValidName("Имя");
        RuleFor(b => b.Surname).IsValidName("Фамилия");
    }
}