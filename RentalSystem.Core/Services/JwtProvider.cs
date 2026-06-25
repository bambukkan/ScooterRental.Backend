using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentalSystem.Core.Models;

public class JwtProvider : IJwtProvider
{
    // Поле для хранения конфигурации (секретный ключ и время жизни токена)
    private readonly JwtOptions _options;

    // Через конструктор запрашиваем IOptions<JwtOptions>. 
    // .NET автоматически прочитает секцию из appsettings.json и замапит её на наш класс.
    public JwtProvider(IOptions<JwtOptions> options)
    {
        // Достаем сам объект с данными из обёртки IOptions
        _options = options.Value;
    }

    public string GenerateToken(UserEntity user)
    {
        // 1. CLAIMS (Полезная нагрузка / Payload)
        // Клеймы — это утверждения о пользователе в формате "ключ-значение".
        // Зашиваем внутрь токена ID пользователя, чтобы при каждом запросе знать, КТО его делает.
        // Данные здесь открыты для чтения, поэтому пароли сюда класть нельзя!
        Claim[] claims = [
            new("UserID", user.Id.ToString()),
            // Используем встроенный тип ClaimTypes.Role, чтобы .NET понимал его автоматически
            new(ClaimTypes.Role,user.Role.ToString())
            ];

        // 2. SIGNING CREDENTIALS (Данные для создания цифровой подписи)
        // Превращаем текстовый секретный ключ из настроек в массив байтов, так как криптография работает только с байтами.
        var keyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);

        // Создаем симметричный ключ безопасности. "Симметричный" означает, что этот же ключ 
        // будет использоваться сервером и для создания токена, и для его проверки в будущем.
        var securityKey = new SymmetricSecurityKey(keyBytes);

        // Упаковываем ключ вместе с алгоритмом шифрования (HMAC SHA-256).
        // С помощью этой связки .NET защитит наш токен от подделки: если хакер изменит UserID, подпись станет невалидной.
        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        // 3. СОЗДАНИЕ ОБЪЕКТА ТОКЕНА
        // Собираем все подготовленные компоненты в единый C#-объект JwtSecurityToken.
        var token = new JwtSecurityToken(
            claims: claims,                                            // Данные юзера (Payload)
            signingCredentials: signingCredentials,                     // Ключ и алгоритм защиты (Signature)
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)   // Время, когда токен "протухнет" (Header/Payload)
        );

        // 4. СЕРИАЛИЗАЦИЯ (Превращение в строку)
        // Сам объект JwtSecurityToken весит много и содержит сложную структуру. Клиенту (фронтенду) нужна строка.
        // JwtSecurityTokenHandler — это встроенный утилитарный класс, который умеет кодировать токен.
        // Метод WriteToken берет наш объект, кодирует Хедер и Пэйлоад в Base64, считает подпись, 
        // склеивает их через точку и выдает финальную строку: "header.payload.signature"
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        // Возвращаем готовую строку-токен (наш "паспорт" или "пропуск"), которую контроллер отдаст клиенту.
        return tokenValue;
    }
}