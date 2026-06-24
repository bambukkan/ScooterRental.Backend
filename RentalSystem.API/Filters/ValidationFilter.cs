using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
public class ValidationFilter : IAsyncActionFilter
{
    private readonly ILogger<ValidationFilter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(ILogger<ValidationFilter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        /* ActionArguments - словарь, в которой мы просто получаем аргументы из метода
        Их значение это сам допустим экземпляр какого то класса или конкретное число/строка, смотря что прилетает
        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductDto dto, [FromQuery] int categoryId)
        {
            // ...
        }
        "dto"	Объект типа CreateProductDto (со всеми заполненными полями)
        "categoryId"	Число 5 (тип int)*/
        foreach(var argument in context.ActionArguments.Values)
        {
            if(argument == null) continue;

            // 1. Узнаем тип DTO, например: CreateProductDto
            var argType = argument.GetType();
            // 2. Создаем тип IValidator<CreateProductDto>
            var validatorType = typeof(IValidator<>).MakeGenericType(argType);

            // 3. Пытаемся достать валидатор из DI
            /*IValidator<CreateProductDto>. FluentValidation регистрирует валидаторы именно так.
             Контейнер вернет экземпляр твоего класса, например, CreateProductDtoValidator.*/
            var validator = _serviceProvider.GetService(validatorType) as IValidator;
            
            if(validator != null)
            {
                var validationContext = new ValidationContext<object>(argument); // контекст для валидации
                var validationResult = await validator.ValidateAsync(validationContext); // выполняем саму валидацию

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Валидация DTO {DtoType} не прошла", argument.GetType().Name);
                    
                    var errors = validationResult.Errors /*
                    Errors - список ошибок, где каждая ошибка содержит
                    PropertyName — имя поля (например, "Price")
                    ErrorMessage — текст ошибки (например, "Цена должна быть больше 0")
                    */
                        .GroupBy(e => e.PropertyName)
                        // У одного поля может быть несоклько ошибок, поэтому мы все удобно группируем типа
                        //"Email": ["Email обязателен", "Некорректный формат Email"],
                        .ToDictionary(
                            g => g.Key,// Ключ словаря — имя свойства ("Email", "Age")
                            g => g.Select(e => e.ErrorMessage)// Значение — выбираем только тексты ошибок
                                .ToArray()  // Превращаем в массив строк
                        );
                    /*g — это каждая группа (IGrouping). 
                    У неё есть:Key — имя свойства
                    Сама группа — коллекция ошибок этой группы
                    Результат: Dictionary<string, string[]>*/

                    context.Result = new BadRequestObjectResult(new { Errors = errors });
                    return; // Разворачиваем плохой запрос, до контроллера он не дойдёт       
                    /*Итог:
                    Клиент присылает кривые данные → фильтр находит валидатор → 
                    запускает проверку → группирует ошибки → возвращает 400 с понятным JSON'ом. 
                    Контроллер даже не дёргается.
                    */         
                }
            }
        }
        await next();
    }
}