using AuthPractice.Data.Models;

namespace AuthPractice.Api.Services;

public interface IAuthService
{
    Task<(int, string)> LoginUser(LoginModel loginModel);

    Task<(int, string)> RegisterUser(RegistrationModel registrationModel, string role);
}