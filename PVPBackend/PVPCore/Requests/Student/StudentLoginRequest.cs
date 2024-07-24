using System.ComponentModel.DataAnnotations;

namespace PVPCore.Requests.Student;

public class StudentLoginRequest
{
    [Required(ErrorMessage = "The username field cannot be empty")]
    public string Username { get; set; }
    [Required(ErrorMessage = "The password field cannot be empty")]
    public string Password { get; set; }
    [Required(ErrorMessage = "The Classroom code field cannot be empty")]
    public int ClassroomCode { get; set; }
}