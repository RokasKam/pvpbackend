using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using PVPCore.Interfaces.Repositories;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Teacher;
using PVPCore.Response.Teacher;
using PVPDomain.Entities;

using ValidationException = PVPDomain.Exceptions.ValidationException;

namespace PVPCore.Services;

public class TeacherAuthService : ITeacherAuthService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private static readonly Encoding HashEncoding = Encoding.UTF8;

    public TeacherAuthService(ITeacherRepository teacherRepository, IMapper mapper, IPasswordService passwordService, IJwtService jwtService)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public JwtResponse Login(TeacherLoginRequest teacherLogin)
    {
        var teacher = _teacherRepository.GetByEmailOrDefault(teacherLogin.Email);

        if (teacher is null)
        {
            throw new ValidationException("Incorrect teacher email");
        }

        using var hmac = new HMACSHA512(teacher.PasswordSalt);
        var computedHash = hmac.ComputeHash(HashEncoding.GetBytes(teacherLogin.Password));

        if (!computedHash.SequenceEqual(teacher.PasswordHash))
        {
            throw new ValidationException("Incorrect teacher password");
        }

        var response = _mapper.Map<TeacherResponse>(teacher);
        var accessToken = _jwtService.BuildJwt(response.Id, "Teacher");
        var refreshToken = _jwtService.GenerateRefreshToken();
        _teacherRepository.UpdateRefreshToken(teacher.Id, refreshToken);
        return new JwtResponse() { 
            AccessToken = accessToken, 
            RefreshToken = refreshToken };
    }

    public Guid Register(TeacherRegisterRequest teacherRegister)
    {
        var registerTeacher = _mapper.Map<Teacher>(teacherRegister);
        var existingTeacher = _teacherRepository.GetByEmailOrDefault(registerTeacher.Email);
        if (existingTeacher is not null)
        {
            throw new ValidationException("Teacher with this email exists");
        }
        var password = _passwordService.HashPassword(teacherRegister.Password);
        registerTeacher.PasswordHash = password.PasswordHash;
        registerTeacher.PasswordSalt = password.PasswordSalt; 
        var user = _teacherRepository.PostTeacher(registerTeacher);
        return user.Id;
    }

    public JwtResponse Refresh(JwtResponse tokens)
    {
        string accessToken = tokens.AccessToken;
        string refreshToken = tokens.RefreshToken;

        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var id = Guid.Parse(principal.FindFirst(ClaimTypes.Sid).Value);
        var teacher = _teacherRepository.GetByIdOrDefault(id);
        if (teacher == null || teacher.RefreshToken != refreshToken || teacher.RefreshTokenExpiryTime <= DateTime.Now)
            throw new ValidationException("Something went wrong with refresh token");
        
        var newAccessToken = _jwtService.BuildJwt(teacher.Id, "Teacher");
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        
        _teacherRepository.UpdateRefreshToken(teacher.Id, newRefreshToken);
        
        return new JwtResponse() { 
            AccessToken = newAccessToken, 
            RefreshToken = newRefreshToken };
    }
}