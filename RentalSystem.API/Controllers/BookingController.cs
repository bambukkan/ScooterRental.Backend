


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
    public async Task<IActionResult> Add([FromBody] CreatingBookingRequest request)
    {
        var BookingId = await _service.Add(request);
        return Ok(BookingId);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.Delete(id);
        return Ok(id);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        await _service.Update(id);
        return Ok(id);
    }
    [HttpGet("ListByUserID")]
    public async Task<IActionResult> GetListByUserId(Guid userId)
    {
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