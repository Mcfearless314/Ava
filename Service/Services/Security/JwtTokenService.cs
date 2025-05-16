using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Service.Services.Security;

public class JwtTokenService
{
  private readonly string _secretKey;
  private readonly string _issuer;
  private readonly string _audience;

  public JwtTokenService(string secretKey, string issuer, string audience)
  {
    _secretKey = secretKey;
    _issuer = issuer;
    _audience = audience;
  }

  public string GenerateJwtToken(string username, Guid userId)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Name, username),
      new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _issuer,
      audience: _audience,
      claims: claims,
      expires: DateTime.Now.AddMinutes(30),
      signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
