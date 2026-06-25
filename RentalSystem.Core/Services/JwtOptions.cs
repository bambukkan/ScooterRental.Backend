public class JwtOptions
{
    public string SecretKey {get;set;} = string.Empty;

    public int ExpiresHours {get;set;} // раз в сколько часов будет обновляться токен
}