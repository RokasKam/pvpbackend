namespace PVPDomain.Entities;

public class Classroom : BaseEntity
{
    public String Classname { get; set; }
    public int Code { get; set; }
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<Student> Students { get; set; }
}