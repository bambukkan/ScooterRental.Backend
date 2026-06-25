public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        // хэшируем наш пароль, который мы уже будем передавать в базу данных
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool Verify(string password,string hashedPassword)
    {
        // сравнием пароль который пришел и хэшируем его с хэшированным паролем
        return BCrypt.Net.BCrypt.EnhancedVerify(password,hashedPassword);
    }
}