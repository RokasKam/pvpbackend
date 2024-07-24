using System.ComponentModel.DataAnnotations;

namespace PVPCore.Requests.Teacher;

public class TeacherRegisterRequest
{

    [Required(ErrorMessage = "The Name field cannot be empty")]
    public String Name { get; set; }
    [Required(ErrorMessage = "The Surname field cannot be empty")]
    public String Surname { get; set; }
    [Required(ErrorMessage = "The email field cannot be empty")]
    [EmailAddress(ErrorMessage = "The email must be valid (example: user@example.com)")]
    public string Email { get; set; }
    [Required(ErrorMessage = "The password field cannot be empty")]
    public string Password { get; set; }
    [Required(ErrorMessage = "The School field cannot be empty")]
    public String School { get; set; }
}