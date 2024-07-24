using PVPCore.Interfaces.Repositories;
using PVPDomain.Entities;
using PVPInfrastructure.Data;

namespace PVPInfrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly PvpDataContext _dbContext;

    public StudentRepository(PvpDataContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Student? GetByUsernameOrDefault(string username, Guid ClassroomId)
    {
        var student = _dbContext.Students.FirstOrDefault(u => 
            u.Username == username && u.ClassroomId == ClassroomId);
        return student;
    }

    public Student PostStudent(Student student)
    {
        student.Id = Guid.NewGuid();
        student.RefreshToken = "";
        student.RefreshTokenExpiryTime = new DateTime();
        _dbContext.Students.Add(student); 
        _dbContext.SaveChanges();
        return student;
    }

    public Student? GetByIdOrDefault(Guid id)
    {
        var student = _dbContext.Students.FirstOrDefault(u => u.Id == id);
        return student; 
    }
    
    public bool UpdateRefreshToken(Guid id, string token)
    {
        var local = _dbContext.Students.First(oldEntity => oldEntity.Id == id);
        local.RefreshToken = token;
        local.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        _dbContext.SaveChanges();
        return true;
    }
}