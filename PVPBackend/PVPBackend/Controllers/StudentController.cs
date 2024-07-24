using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Student;
using PVPCore.Response.Teacher;

namespace PVPBackend.Controllers;

public class StudentController : BaseController
{
    private readonly IStudentAuthService _studentAuthService;
    private readonly IJwtService _jwtService;
    private readonly IStudentService _studentService;

    public StudentController(IStudentAuthService studentAuthService, IJwtService jwtService, IStudentService studentService)
    {
        _studentAuthService = studentAuthService;
        _jwtService = jwtService;
        _studentService = studentService;
    }
    
    [HttpPost]
    public IActionResult Login(StudentLoginRequest request)
    {
        var jwtResponse = _studentAuthService.Login(request);
        return Ok(jwtResponse);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public IActionResult Register(StudentRegisterRequest request)
    {
        var response = _studentAuthService.Register(request);
        return Ok(response);
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
    public IActionResult GetStudent()
    {
        var student = _studentService.GetById(Guid.Parse(User.FindFirstValue(ClaimTypes.Sid)));
        return Ok(student);
    }
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
    public IActionResult Refresh(JwtResponse tokens)
    {
        var jwtResponse = _studentAuthService.Refresh(tokens); 
        return Ok(jwtResponse);
    }
}