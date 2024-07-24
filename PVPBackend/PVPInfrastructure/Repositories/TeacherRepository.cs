using PVPCore.Interfaces.Repositories;
using PVPDomain.Entities;
using PVPInfrastructure.Data;
using PVPInfrastructure.Migrations;

namespace PVPInfrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly PvpDataContext _dbContext;

    public TeacherRepository(PvpDataContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Teacher? GetByEmailOrDefault(string email)
    {
        var teacher = _dbContext.Teachers.FirstOrDefault(u => u.Email == email);
        return teacher;
        
    }

    public Teacher PostTeacher(Teacher user)
    {
        user.Id = Guid.NewGuid();
        user.RefreshToken = "";
        user.RefreshTokenExpiryTime = new DateTime();
        _dbContext.Teachers.Add(user); 
        _dbContext.SaveChanges();
        return user;
    }

    public Teacher? GetByIdOrDefault(Guid id)
    {
        var teacher = _dbContext.Teachers.FirstOrDefault(u => u.Id == id);
        return teacher; 
    }

    public bool UpdateRefreshToken(Guid id, string token)
    {
        var local = _dbContext.Teachers.First(oldEntity => oldEntity.Id == id);
        local.RefreshToken = token;
        local.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        _dbContext.SaveChanges();
        return true;
    }
}