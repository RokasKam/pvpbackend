using PVPDomain.Entities;

namespace PVPCore.Interfaces.Repositories;

public interface IStudentRepository
{
    Student? GetByUsernameOrDefault(string username, Guid ClassroomId);
    Student PostStudent(Student student);
    Student? GetByIdOrDefault(Guid id);
    bool UpdateRefreshToken(Guid id, string token);
}