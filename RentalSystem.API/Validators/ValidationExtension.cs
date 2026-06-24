using FluentValidation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> IsValidName<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{fieldName} обязательно для заполнения")
            .MinimumLength(2).WithMessage($"Минимум символов в поле {fieldName} - 2")
            .MaximumLength(40).WithMessage($"Максимум символов в поле {fieldName} - 40");
    }

    public static IRuleBuilderOptions<T, string> IsValidSerialNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Самокат должен иметь серийный номер")
            .Length(7).WithMessage("Длина серийного номера самоката должна быть равна 7");
    }
}