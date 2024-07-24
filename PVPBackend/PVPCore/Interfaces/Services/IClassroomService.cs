using PVPCore.Requests.Classroom;
using PVPCore.Response.Classroom;

namespace PVPCore.Interfaces.Services;

public interface IClassroomService
{
    Guid AddNewClass(ClassroomRequest classroomRequest, Guid teacherId);
    IEnumerable<ClassroomResponse> GetAllTeacherClasses(Guid teacherId);
}