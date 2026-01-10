using Microsoft.AspNetCore.Mvc;
using TheButton.Services;
using TheButton.Api.Models;
using Asp.Versioning;

namespace TheButton.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/counter")]
public class CounterController : ControllerBase
{
    private readonly ICounterService _counterService;

    public CounterController(ICounterService counterService)
    {
        _counterService = counterService;
    }

    [HttpPost]
    public IActionResult Increment()
    {
        var newValue = _counterService.Increment();
        return Ok(new CounterResponse(newValue));
    }
}
