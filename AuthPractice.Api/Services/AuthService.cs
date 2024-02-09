using AuthPractice.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace AuthPractice.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthService(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_dbContext = appDbContext;
		_roleManager = roleManager;
		_userManager = userManager;
	}

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

		if(await _roleManager.RoleExistsAsync(UserRoles.User))
			await _userManager.AddToRoleAsync(user, role);

		return (1, "Created Successfully");

	}
}

