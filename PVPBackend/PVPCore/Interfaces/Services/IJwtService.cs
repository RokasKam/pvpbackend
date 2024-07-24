using System.Security.Claims;
using PVPCore.Response.Teacher;

namespace PVPCore.Interfaces.Services;

public interface IJwtService
{
    String BuildJwt(Guid user, string role);
    String GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}