using PVPCore.Requests.Teacher;
using PVPCore.Response.Teacher;

namespace PVPCore.Interfaces.Services;

public interface ITeacherAuthService
{
    JwtResponse Login(TeacherLoginRequest teacherLogin);

    Guid Register(TeacherRegisterRequest teacherRegister);
    JwtResponse Refresh(JwtResponse tokens);
}