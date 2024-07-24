using System.ComponentModel.DataAnnotations;

namespace PVPCore.Requests.Classroom;

public class ClassroomRequest
{
    [Required(ErrorMessage = "The Classroom name field cannot be empty")]
    public string Classname { get; set; }
}