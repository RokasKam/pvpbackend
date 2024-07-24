using PVPCore.Requests.Student;
using PVPCore.Response.Student;
using PVPCore.Response.Teacher;

namespace PVPCore.Interfaces.Services;

public interface IStudentAuthService
{
    JwtResponse Login(StudentLoginRequest login);
    Guid Register(StudentRegisterRequest register);
    JwtResponse Refresh(JwtResponse tokens);
}