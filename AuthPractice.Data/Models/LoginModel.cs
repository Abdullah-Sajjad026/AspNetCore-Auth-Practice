using System.ComponentModel.DataAnnotations;

namespace AuthPractice.Data.Models;

public class LoginModel
{

    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}

