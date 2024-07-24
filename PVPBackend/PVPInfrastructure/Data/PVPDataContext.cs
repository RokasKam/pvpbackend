using Microsoft.EntityFrameworkCore;
using PVPDomain.Entities;
using PVPInfrastructure.Data.Configuration;

namespace PVPInfrastructure.Data;

public class PvpDataContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Student> Students { get; set; }

    public PvpDataContext(DbContextOptions<PvpDataContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeacherConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClassroomConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentConfiguration).Assembly);
    }
}