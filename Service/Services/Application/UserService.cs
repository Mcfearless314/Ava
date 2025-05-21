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
      var userWithCredentials = await _userRepository.CheckIfUserExistsByUsername(username);

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
      if (e.Message == "User already exists.")
      {
        throw;
      }

      throw new Exception("Registration failed.");
    }
  }

  public async Task<User> Authenticate(string username, string password)
  {
    var userWithCredentials = await _userRepository.GetUserWithCredentialsByUsername(username);

    if (userWithCredentials == null || userWithCredentials.Credentials == null)
    {
      throw new Exception("Invalid username or password.");
    }

    var credentials = userWithCredentials.Credentials;

    if (!_passwordHashAlgorithm.VerifyHashedPassword(username, password, credentials.PasswordHash, credentials.Salt))
    {
      throw new Exception("Invalid username or password.");
    }

    return await _userRepository.GetUserById(credentials.UserId);
  }


  public async Task<List<User>> GetAllUsers(Guid organisationId)
  {
    try
    {
      return await _userRepository.GetAllUsers(organisationId);
    }
    catch (Exception)
    {
      throw new Exception("Failed to get users by organisation.");
    }
  }

  public async Task<List<User>> GetUsersByProject(Guid projectId)
  {
    try
    {
      return await _userRepository.GetUsersByProject(projectId);
    }
    catch (Exception e)
    {
      if (e is KeyNotFoundException)
      {
        return new List<User>();
      }
      throw new Exception("Failed to get users by project.");
    }
  }

  public async Task<User> UpdateUser(Guid userId, string username, string password)
  {
    try
    {
      var existingUser = await _userRepository.GetUserById(userId);

      if (existingUser == null)
      {
        throw new Exception("User not found.");
      }

      var salt = _passwordHashAlgorithm.GenerateSalt();
      var passwordHash = _passwordHashAlgorithm.HashPassword(password, salt);

      return await _userRepository.UpdateUser(userId, username, salt, passwordHash);
    }
    catch (Exception e)
    {
      throw new Exception("Update failed.", e);
    }
  }

  public async Task DeleteUser(Guid userId)
  {
    try
    {
      await _userRepository.DeleteUser(userId);
    }
    catch (Exception e)
    {
      throw new Exception("Delete failed.", e);
    }
  }

  public async Task<Guid?> GetOrganisationId(Guid userId)
  {
    try
    {
      return await _userRepository.GetOrganisationId(userId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to get organisation ID.", e);
    }
  }

  public async Task<User?> GetUserByUsername(string username)
  {
    return await _userRepository.GetUserByUsername(username);
  }
}
