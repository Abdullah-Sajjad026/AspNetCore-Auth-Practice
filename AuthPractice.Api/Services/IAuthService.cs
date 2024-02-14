using AuthPractice.Data.Models;

namespace AuthPractice.Api.Services;

public interface IAuthService
{
    Task<(int, string)> LoginUserAsync(LoginModel loginModel);

    Task<(int, string)> RegisterUserAsync(RegistrationModel registrationModel, string role);
}