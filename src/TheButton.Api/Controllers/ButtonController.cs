using Microsoft.AspNetCore.Mvc;
using TheButton.Services;
using TheButton.Api.Models;

namespace TheButton.Controllers;

[ApiController]
[Route("api/button")]
public class ButtonController : ControllerBase
{
    private readonly ICounterService _counterService;

    public ButtonController(ICounterService counterService)
    {
        _counterService = counterService;
    }



    [HttpPost("click")]
    public IActionResult Click()
    {
        var newValue = _counterService.Increment();
        return Ok(new CounterResponse(newValue));
    }
}
