using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Classroom;

namespace PVPBackend.Controllers;

public class ClassroomController : BaseController
{
    private readonly IClassroomService _classroomService;
    
    public ClassroomController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public IActionResult AddNewClass(ClassroomRequest classroomRequest)
    {
        var classroom = _classroomService.AddNewClass(classroomRequest,
            Guid.Parse(User.FindFirstValue(ClaimTypes.Sid)));
        return Ok(classroom);
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public IActionResult GetAllTeacherClasses()
    {
        var classrooms = _classroomService.GetAllTeacherClasses(
            Guid.Parse(User.FindFirstValue(ClaimTypes.Sid)));
        return Ok(classrooms);
    }
}