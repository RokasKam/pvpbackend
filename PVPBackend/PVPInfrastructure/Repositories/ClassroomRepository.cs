using PVPCore.Interfaces.Repositories;
using PVPDomain.Entities;
using PVPInfrastructure.Data;

namespace PVPInfrastructure.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly PvpDataContext _dataContext;
    private readonly Random _random = new();
    public ClassroomRepository(PvpDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public Guid PostClassroom(Classroom classroom)
    {
        classroom.Id = Guid.NewGuid();
        classroom.Code = _random.Next(100000, 1000000); 
        _dataContext.Classrooms.Add(classroom); 
        _dataContext.SaveChanges();
        return classroom.Id;
    }

    public Classroom? GetByNameTeacherClass(string name, Guid teacherId)
    {
        var classroom = _dataContext.Classrooms.FirstOrDefault(u => 
            u.Classname == name && u.TeacherId == teacherId);
        return classroom;
    }

    public IEnumerable<Classroom> GetAllTeacherClassrooms(Guid teacherId)
    {
        var classrooms = _dataContext.Classrooms.Where(p=>p.TeacherId == teacherId);
        return classrooms.ToList();
    }

    public Classroom? GetByCode(int code)
    {
        var classroom = _dataContext.Classrooms.FirstOrDefault(p => p.Code == code);
        return classroom;
    }
}