using PVPDomain.Entities;

namespace PVPCore.Interfaces.Repositories;

public interface IClassroomRepository
{
    Guid PostClassroom(Classroom classroom);
    Classroom? GetByNameTeacherClass(String name, Guid teacherId);
    IEnumerable<Classroom> GetAllTeacherClassrooms(Guid teacherId);
    Classroom? GetByCode(int code);
}