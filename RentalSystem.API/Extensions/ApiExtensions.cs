using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class ApiExtensions
{

    /*AddApiAuthentication — это, по сути, 
    инструкция для твоего сервера, как проверять входящие паспорта (токены) от пользователей.
    Когда юзер будет запрашивать список самокатов, он прикрепит свой токен.
     Благодаря этому методу, .NET поймет: валидный токен или подделка.
    */
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ШАГ 1: Достаем настройки из appsettings.json
        // Нам физически нужен SecretKey, чтобы .NET знал, с каким ключом сравнивать подпись токена.
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        // ШАГ 2: Включаем систему аутентификации в .NET
        // Мы говорим: "Сервер, при обработке запросов используй схему JwtBearer (то есть проверку JWT токенов)".
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            
            // ШАГ 3: Настраиваем правила проверки токена
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // TokenValidationParameters — это самый главный набор правил ("чек-лист").
                // Когда прилетает токен, .NET берет этот чек-лист и проверяет токен по пунктам:
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 1. Проверять ли, КТО выдал токен (Issuer)? 
                    // Кирилл ставит false для простоты, чтобы не прописывать лишние строки урла нашего сервера.
                    ValidateIssuer = false, 

                    // 2. Проверять ли, ДЛЯ КОГО выдан токен (Audience)? 
                    // Тоже false для простоты пет-проекта.
                    ValidateAudience = false, // сейчас мне не нужен издатель или потребитель, это понадобится если микросервисы,а у меня щас только один сервис

                    // 3. Проверять ли время жизни токена (Lifetime)?
                    // ОБЯЗАТЕЛЬНО true. Если у токена истекли его 12 часов, .NET сразу скажет "401 Unauthorized".
                    ValidateLifetime = true, 

                    // 4. Проверять ли цифровую подпись в конце токена (SigningKey)?
                    // КРИТИЧЕСКИ ВАЖНО true. Без этого хакер сможет подделать UserID.
                    ValidateIssuerSigningKey = true, 

                    // 5. Передаем секретный ключ для проверки подписи
                    // Превращаем наш SecretKey в байты. Именно этим ключом .NET будет "пересчитывать" 
                    // подпись прилетевшего токена, чтобы убедиться, что его сгенерировали мы, а не хакер.
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey)) 
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access-cookies"];
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
    }
}