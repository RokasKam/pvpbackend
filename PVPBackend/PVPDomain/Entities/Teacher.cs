namespace PVPDomain.Entities;

public class Teacher : BaseEntity
{
    public String Name { get; set; }
    public String Surname { get; set; }
    public String Email { get; set; }
    public String School { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public ICollection<Classroom> Classrooms { get; set; }
    public String? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}// class, school
//Children:
//username, password, class
