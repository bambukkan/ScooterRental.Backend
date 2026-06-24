

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
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatingUserRequest request)
    {
        var userId = await _service.Add(request);
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
}