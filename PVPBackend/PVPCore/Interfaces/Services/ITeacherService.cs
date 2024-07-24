using PVPCore.Response.Teacher;

namespace PVPCore.Interfaces.Services;

public interface ITeacherService
{
    TeacherResponse GetById(Guid id);
}