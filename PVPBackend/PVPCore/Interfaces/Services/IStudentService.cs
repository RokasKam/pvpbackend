using PVPCore.Response.Student;

namespace PVPCore.Interfaces.Services;

public interface IStudentService
{
    StudentResponse GetById(Guid id);
}