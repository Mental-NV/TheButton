using Microsoft.AspNetCore.Mvc;
using TheButton.Services;

namespace TheButton.Controllers;

[ApiController]
[Route("api/button")]
public class ButtonController : ControllerBase
{
    private readonly CounterService _counterService;

    public ButtonController(CounterService counterService)
    {
        _counterService = counterService;
    }

    [HttpPost("click")]
    public IActionResult Click()
    {
        var newValue = _counterService.Increment();
        return Ok(new { value = newValue });
    }
}
