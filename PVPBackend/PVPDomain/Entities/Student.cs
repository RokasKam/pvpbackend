namespace PVPDomain.Entities;

public class Student : BaseEntity
{
    public String Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
    public String? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}