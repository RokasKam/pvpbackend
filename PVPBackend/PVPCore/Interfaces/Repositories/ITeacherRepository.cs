using PVPDomain.Entities;

namespace PVPCore.Interfaces.Repositories;

public interface ITeacherRepository
{
    Teacher? GetByEmailOrDefault(string email);
    Teacher PostTeacher(Teacher user);
    Teacher? GetByIdOrDefault(Guid id);
    bool UpdateRefreshToken(Guid id, string token);
}