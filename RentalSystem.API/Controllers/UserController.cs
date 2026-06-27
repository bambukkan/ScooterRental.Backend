

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok( await _service.Get());
    }
    [HttpGet("by-id")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok( await _service.GetById(id));
    }
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        return Ok( await _service.GetByEmail(email));
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatingUserRequest request)
    {
        var userId = await _service.Add(request);
        return Ok(userId);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var (token,refreshToken) =  await _service.Login(request);

        Response.Cookies.Append("Access-cookies",token,new CookieOptions
        {
            HttpOnly = true, // Внутри сайта можно передавать только по сети в http-заголовках
            Secure = true, // куки передается только по HTTPS
            SameSite = SameSiteMode.Strict // Если нет этой строчки,то
            // Если ты зайдешь на сторонний сайт хакера, то он может обратиться к твоему сайту
            // Хакер куку не видит, но его запрос выполняется под твоим именем
        });
        Response.Cookies.Append("Refresh-cookies",refreshToken,new CookieOptions
        {
            HttpOnly = true, // Внутри сайта можно передавать только по сети в http-заголовках
            Secure = true, // куки передается только по HTTPS
            SameSite = SameSiteMode.Strict // Если нет этой строчки,то
            // Если ты зайдешь на сторонний сайт хакера, то он может обратиться к твоему сайту
            // Хакер куку не видит, но его запрос выполняется под твоим именем
        });

        return Ok(token);
    }
    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh()
    {
        string? refreshToken = Request.Cookies["Refresh-cookies"];
        if(refreshToken == null)
        {
            return Unauthorized();
        }

        var (newToken,newRefreshToken,userId) = await _service.Refresh(refreshToken);

        Response.Cookies.Append("Access-cookies",newToken,new CookieOptions
        {
            HttpOnly = true, 
            Secure = true, 
            SameSite = SameSiteMode.Strict 
        });
        Response.Cookies.Append("Refresh-cookies",newRefreshToken,new CookieOptions
        {
            HttpOnly = true, 
            Secure = true, 
            SameSite = SameSiteMode.Strict 
        });

        return Ok(userId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.Delete(id);
        return Ok(id);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateUserRequest request)
    {
        await _service.Update(id,request);
        return Ok(id);
    }

    [HttpPut("promote-to-admin")]
    public async Task<IActionResult> PromoteToAdmin([FromQuery] Guid userId, [FromHeader] string masterKey)
    {
        // Зашиваешь секретную строку прямо в код (или в appsettings.json)
        if (masterKey != "MySuperSecretDeveloperPassword123")
        {
            return Forbid(); // Если ключ не совпал — давай до свидания
        }

        // Если ключ верный, вызываем сервис для смены роли
        await _service.ChangeRole(userId, UserRole.Admin);
        return Ok("Теперь ты админ!");
    }
}