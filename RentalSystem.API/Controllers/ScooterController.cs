

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Scooters")]
public class ScooterController : ControllerBase
{
    private readonly IScooterService _service;

    public ScooterController(IScooterService service)
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
    [HttpGet("by-SerialNumber")]
    public async Task<IActionResult> GetBySerialNumber([FromBody] string serNum)
    {
        return Ok( await _service.GetBySerialNumber(serNum));
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatingScooterRequest request)
    {
        var scooterId = await _service.Add(request);
        return Ok(scooterId);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.Delete(id);
        return Ok(id);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateScooterRequest request)
    {
        await _service.Update(id,request);
        return Ok(id);
    }
}