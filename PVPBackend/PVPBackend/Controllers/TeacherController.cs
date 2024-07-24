using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Teacher;
using PVPCore.Response.Teacher;

namespace PVPBackend.Controllers;

public class TeacherController : BaseController
{
    private readonly ITeacherAuthService _teacherAuthService;
    private readonly ITeacherService _teacherService;
    private readonly IJwtService _jwtService;
    
    public TeacherController(ITeacherAuthService teacherAuthService, ITeacherService teacherService, IJwtService jwtService)
    {
        _teacherAuthService = teacherAuthService;
        _teacherService = teacherService;
        _jwtService = jwtService;
    }

    [HttpPost]
    public IActionResult Login(TeacherLoginRequest request)
    {
        var jwtResponse = _teacherAuthService.Login(request);
        return Ok(jwtResponse);
    }

    [HttpPost]
    public IActionResult Register(TeacherRegisterRequest request)
    {
        var response = _teacherAuthService.Register(request);
        return Ok(response);
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public IActionResult GetTeacher()
    {
        var teacher = _teacherService.GetById(Guid.Parse(User.FindFirstValue(ClaimTypes.Sid)));
        return Ok(teacher);
    }
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
    public IActionResult Refresh(JwtResponse tokens)
    {
        var jwtResponse = _teacherAuthService.Refresh(tokens); 
        return Ok(jwtResponse);
    }
}