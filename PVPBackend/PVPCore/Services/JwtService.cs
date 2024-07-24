using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PVPCore.ApiSettings;
using PVPCore.Interfaces.Services;

namespace PVPCore.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    
    private const string HashAlgorithm = SecurityAlgorithms.HmacSha256Signature;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SymmetricSecurityKey _securityKey;
    
    public JwtService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        _tokenHandler = new JwtSecurityTokenHandler();
        _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
    }
    
    public String BuildJwt(Guid user, string role)
    {
        var tokenDescriptor = CreateAccessTokenDescriptor(user, role);
        var securityToken = _tokenHandler.CreateToken(tokenDescriptor);
        
        return _tokenHandler.WriteToken(securityToken);
    }
    public String GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _securityKey,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
    private SecurityTokenDescriptor CreateAccessTokenDescriptor(Guid user, string role)
    {
        var claims = new List<Claim>
        {			
            new(ClaimTypes.Sid, user.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, role.ToString()),
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.Add(_jwtSettings.TokenLifetime),
            SigningCredentials = new SigningCredentials(_securityKey, HashAlgorithm),
        };

        return tokenDescriptor;
    }
}