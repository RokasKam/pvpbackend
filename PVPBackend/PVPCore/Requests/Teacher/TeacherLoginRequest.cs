using System.ComponentModel.DataAnnotations;

namespace PVPCore.Requests.Teacher;

public class TeacherLoginRequest
{
    [Required(ErrorMessage = "The email field cannot be empty")]
    [EmailAddress(ErrorMessage = "The email must be valid (example: user@example.com)")]
    public string Email { get; set; }
    [Required(ErrorMessage = "The password field cannot be empty")]
    public string Password { get; set; }
}