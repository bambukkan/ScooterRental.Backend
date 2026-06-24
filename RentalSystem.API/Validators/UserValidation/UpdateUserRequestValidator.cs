using FluentValidation;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
         RuleFor(b => b.Name).IsValidName("Имя");
         RuleFor(b => b.Surname).IsValidName("Фамилия");
    }
}