using FluentValidation;

public class CreatingUserRequestValidator : AbstractValidator<CreatingUserRequest>
{
    public CreatingUserRequestValidator()
    {
        RuleFor(b => b.Name).IsValidName("Имя");
        RuleFor(b => b.Surname).IsValidName("Фамилия");


        RuleFor(b => b.Email)
            .NotEmpty().WithMessage("Email не должен быть пустым")
            .EmailAddress().WithMessage("Неверный email или пароль");
        RuleFor(b => b.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов")
            .MaximumLength(32).WithMessage("Пароль не может содержать больше 32 символов")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.");
    }
}