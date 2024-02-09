using AuthPractice.Data.Models;
using AuthPractice.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace AuthPractice.Api.Controllers;


[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser(RegistrationModel registerationModel)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Payload");

            var (status, message) = await _authService.RegisterUser(registerationModel, UserRoles.User);

            if (status is 0)
                return BadRequest(message);

            return CreatedAtAction(nameof(RegisterUser), message);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }
}

