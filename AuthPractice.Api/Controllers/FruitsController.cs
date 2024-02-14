using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthPractice.Api.Controllers;

[Route("api/fruits")]
[Authorize]
[ApiController]
public class FruitsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var fruits = await Task.FromResult(new string[] { "apple", "bananana", "kiwi" });
        return Ok(fruits);
    }
}