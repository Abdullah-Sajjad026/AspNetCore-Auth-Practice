using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthPractice.Data.Models;

/// <summary>
/// Provides a database context for the AuthPractice application using Entity Framework Core and Identity Framework.
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>
{

    /// <summary>
    /// Initializes a new instance of the AppDbContext class with the specified DbContext options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
	{
        // The base constructor handles initializing the DbContext with the provided options.
    }

}

