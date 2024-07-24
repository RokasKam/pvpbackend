using System.Security.Cryptography;
using System.Text;
using PVPCore.Interfaces.Services;

namespace PVPCore.Services;

public class PasswordService : IPasswordService
{
    private static readonly Encoding HashEncoding = Encoding.UTF8;
    public HashPasswordResponse HashPassword(string password)
    {
        using var hmac = new HMACSHA512();

        var response = new HashPasswordResponse(
            PasswordSalt: hmac.Key,
            PasswordHash: hmac.ComputeHash(HashEncoding.GetBytes(password))
        );

        return response;
    }
}