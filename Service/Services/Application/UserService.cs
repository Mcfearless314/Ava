using Service.Services.Security;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Services.Application;

public class UserService
{
  private readonly IUserRepository _userRepository;
  private readonly PasswordHashAlgorithm _passwordHashAlgorithm;

  public UserService(IUserRepository userRepository, PasswordHashAlgorithm passwordHashAlgorithm)
  {
    _userRepository = userRepository;
    _passwordHashAlgorithm = passwordHashAlgorithm;
  }

  public async Task Register(string username, string password)
  {
    try
    {
      var userWithCredentials = await _userRepository.GetUserByUsername(username);

      if (userWithCredentials)
      {
        throw new Exception("User already exists.");
      }

      var salt = _passwordHashAlgorithm.GenerateSalt();
      var passwordHash = _passwordHashAlgorithm.HashPassword(password, salt);

      await _userRepository.Register(username, passwordHash, salt);
    }
    catch (Exception e)
    {
      throw new Exception("Registration failed.", e);
    }
  }

  public async Task<User> Authenticate(string username, string password)
  {
    try
    {
      var userWithCredentials = await _userRepository.GetUserWithCredentialsByUsername(username);

      if (userWithCredentials == null || userWithCredentials.Credentials == null)
      {
        throw new Exception("User or credentials not found.");
      }

      var credentials = userWithCredentials.Credentials;

      if (_passwordHashAlgorithm.VerifyHashedPassword(username, password, credentials.PasswordHash, credentials.Salt))
      {
        return await _userRepository.GetUserById(credentials.UserId);
      }

      throw new Exception("Invalid username or password.");
    }
    catch (Exception e)
    {
      throw new Exception("Authentication failed.", e);
    }
  }
}
