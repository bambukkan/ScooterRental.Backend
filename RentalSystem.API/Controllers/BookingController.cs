


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("Bookings")]

public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok( await _service.Get());
    }
    [HttpGet("With-Details")]
    public async Task<IActionResult> GetWithDetails()
    {
        return Ok( await _service.GetWithDetails());
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] CreatingBookingRequest request)
    {
        // 1. Достаем строковое значение клейма "UserID", который мы зашивали в JwtProvider
        var userIdClaim = User.FindFirst("UserID")?.Value;
        
        if (userIdClaim == null)
        {
            return Unauthorized(); // Если токена нет или он битый
        }

        Guid userId = Guid.Parse(userIdClaim);

        var BookingId = await _service.Add(userId,request);
        return Ok(BookingId);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.Delete(id);
        return Ok(id);
    }
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(Guid bookingId)
    {
        var userIdClaim = User.FindFirst("UserID")?.Value;
        
        if (userIdClaim == null)
        {
            return Unauthorized(); 
        }

        Guid userId = Guid.Parse(userIdClaim);
        await _service.Update(bookingId,userId);
        return Ok(bookingId);
    }
    [HttpGet("ListByUserID")]
    public async Task<IActionResult> GetListByUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(); 
        }
        Guid userId = Guid.Parse(userIdClaim);
        return Ok( await _service.GetListByUserId(userId));
    }
    [HttpGet("ByScooterID")]
    public async Task<IActionResult> GetByScooterId(Guid scooterId)
    {
        return Ok( await _service.GetByScooterId(scooterId));
    }
    [HttpGet("ByID")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok( await _service.GetById(id));
    }
}