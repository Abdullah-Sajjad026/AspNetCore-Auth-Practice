using AuthPractice.Data.Models;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthPractice.Api.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
	{
		_roleManager = roleManager;
		_userManager = userManager;
		_configuration = configuration;
	}

	/// <summary>
	/// Registers a new user in the system with the given role and basic user role.
	/// </summary>
	/// <param name="role">Make sure that this value has some restrictions like using from a static class or some type of enum.</param>
	public async Task<(int, string)> RegisterUser(RegistrationModel registrationModel, string role)
	{
		// Checking if user already exists by username
		var userExists = await _userManager.FindByNameAsync(registrationModel.UserName);


		if(userExists is not null)
		{
			return (0, "User already exists");
		}

		// If not exists, bootstrap a new user with necessary information
		AppUser user = new AppUser()
		{
			Email = registrationModel.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
			UserName = registrationModel.UserName,
			Name = registrationModel.Name
		};

		// Try to create new user with password
		var userCreated = await _userManager.CreateAsync(user, registrationModel.Password);

		if(!userCreated.Succeeded)
		{
			return (0, "Failed");
		}

		// Now adding the provided role to user.
		// Checking if role already exists in identity store

		if(!(await _roleManager.RoleExistsAsync(role)))
		{
			await _roleManager.CreateAsync(new IdentityRole(role));
		}

		if (!(await _roleManager.RoleExistsAsync(UserRoles.User)))
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

		await _userManager.AddToRoleAsync(user, role);

		return (1, "Created Successfully");
	}

	public async Task<(int, string)> LoginUser(LoginModel loginModel)
	{
		var user = await _userManager.FindByNameAsync(loginModel.UserName);

		if(user is null)
			return (0, "Invalid Credentials");

		if (!(await _userManager.CheckPasswordAsync(user, loginModel.Password)))
			return (0, "Invalid Credentials");

		var userRoles = await _userManager.GetRolesAsync(user);
		var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		foreach (var role in userRoles)
			authClaims.Add(new Claim(ClaimTypes.Role, role));

		string token =  GenerateToken(authClaims);
		return (1, token);
	}

	private string GenerateToken(List<Claim> claims)
	{
		var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Audience = _configuration["JWT:Audience"],
			Issuer = _configuration["JWT:Issuer"],
			Subject = new ClaimsIdentity(claims),
			SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
			Expires = DateTime.Now.AddHours(6)
		};

        var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);


    }
}

