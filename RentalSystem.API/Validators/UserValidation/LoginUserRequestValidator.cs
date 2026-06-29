
using FluentValidation;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(b => b.Email)
            .NotEmpty().WithMessage("Email не должен быть пустым")
            .EmailAddress().WithMessage("Неверный email или пароль");

        RuleFor(b => b.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения.");
    }
}