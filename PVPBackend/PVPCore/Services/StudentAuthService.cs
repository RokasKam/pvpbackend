using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using PVPCore.Interfaces.Repositories;
using PVPCore.Interfaces.Services;
using PVPCore.Requests.Student;
using PVPCore.Response.Student;
using PVPCore.Response.Teacher;
using PVPDomain.Entities;

using ValidationException = PVPDomain.Exceptions.ValidationException;

namespace PVPCore.Services;

public class StudentAuthService : IStudentAuthService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private static readonly Encoding HashEncoding = Encoding.UTF8;
    
    public StudentAuthService(IStudentRepository studentRepository, IClassroomRepository classroomRepository, IMapper mapper, IPasswordService passwordService, IJwtService jwtService)
    {
        _studentRepository = studentRepository;
        _classroomRepository = classroomRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }
    
    public JwtResponse Login(StudentLoginRequest login)
    {
        var classroom = _classroomRepository.GetByCode(login.ClassroomCode);
        if (classroom is null)
        {
            throw new ValidationException("Class with such code does not exist");
        }
        
        var student = _studentRepository.GetByUsernameOrDefault(login.Username, classroom.Id);
        if (student is null)
        {
            throw new ValidationException("Student with such username does not exist in classroom");
        }
        
        using var hmac = new HMACSHA512(student.PasswordSalt);
        var computedHash = hmac.ComputeHash(HashEncoding.GetBytes(login.Password));
        if (!computedHash.SequenceEqual(student.PasswordHash))
        {
            throw new ValidationException("Incorrect student password");
        }
        
        var response = _mapper.Map<StudentResponse>(student);
        var accessToken = _jwtService.BuildJwt(response.Id, "Student");
        var refreshToken = _jwtService.GenerateRefreshToken();
        _studentRepository.UpdateRefreshToken(student.Id, refreshToken);
        return new JwtResponse() { 
            AccessToken = accessToken, 
            RefreshToken = refreshToken };
    }

    public Guid Register(StudentRegisterRequest register)
    {
        var classroom = _classroomRepository.GetByCode(register.ClassroomCode);
        if (classroom is null)
        {
            throw new ValidationException("Class with such code does not exist");
        }
        var registerStudent = _mapper.Map<Student>(register);
        var existingStudent = _studentRepository.GetByUsernameOrDefault(registerStudent.Username, classroom.Id);
        if (existingStudent is not null)
        {
            throw new ValidationException("Student with this username exists in this classroom");
        }
        var password = _passwordService.HashPassword(register.Password);
        registerStudent.PasswordHash = password.PasswordHash;
        registerStudent.PasswordSalt = password.PasswordSalt;
        registerStudent.ClassroomId = classroom.Id;
        var user = _studentRepository.PostStudent(registerStudent);
        return user.Id;
    }

    public JwtResponse Refresh(JwtResponse tokens)
    {
        string accessToken = tokens.AccessToken;
        string refreshToken = tokens.RefreshToken;

        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var id = Guid.Parse(principal.FindFirst(ClaimTypes.Sid).Value);
        var student = _studentRepository.GetByIdOrDefault(id);
        if (student == null || student.RefreshToken != refreshToken || student.RefreshTokenExpiryTime <= DateTime.Now)
            throw new ValidationException("Something went wrong with refresh token");
        
        var newAccessToken = _jwtService.BuildJwt(student.Id, "Student");
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        
        _studentRepository.UpdateRefreshToken(student.Id, newRefreshToken);
        
        return new JwtResponse() { 
            AccessToken = newAccessToken, 
            RefreshToken = newRefreshToken
            
        };
        
    }
}