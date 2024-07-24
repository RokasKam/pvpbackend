namespace PVPCore.Interfaces.Services;
public record HashPasswordResponse(byte[] PasswordHash, byte[] PasswordSalt);

public interface IPasswordService
{
    HashPasswordResponse HashPassword(string password);
}