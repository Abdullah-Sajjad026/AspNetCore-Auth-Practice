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

    
    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin(RegistrationModel registerationModel)
    {
        return await RegisterUserInternal(registerationModel, UserRoles.Admin);
        
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser(RegistrationModel registrationModel)
    {
        return await RegisterUserInternal(registrationModel, UserRoles.User);
    }

    private async Task<IActionResult> RegisterUserInternal(RegistrationModel model, string role)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Payload");

            var (status, message) = await _authService.RegisterUser(model, role);

            if (status == 0)
                return BadRequest(message);

            return CreatedAtAction(nameof(RegisterUser), message);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        try
        {
            var (status, message) = await _authService.LoginUser(loginModel);
            if (status == 0)
                return BadRequest(message);

            return Ok(message);
        } catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

