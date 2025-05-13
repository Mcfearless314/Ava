using System.Security.Cryptography;

namespace Service.Services.Security;

public abstract class PasswordHashAlgorithm
{
  public abstract string HashPassword(string password, string salt);

  public abstract bool VerifyHashedPassword(string email, string password, string hash, string salt);

  public string GenerateSalt()
  {
    return Encode(RandomNumberGenerator.GetBytes(32));
  }

  protected byte[] Decode(string value)
  {
    return Convert.FromBase64String(value);
  }

  protected string Encode(byte[] value)
  {
    return Convert.ToBase64String(value);
  }
}
