using System.ComponentModel.DataAnnotations;


namespace AuthPractice.Data.Models;

/// <summary>
/// Registration model of the app
/// </summary>
public class RegistrationModel
{

    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

