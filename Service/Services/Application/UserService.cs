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

      if (userWithCredentials == null)
      {
        throw new Exception("Invalid username or password.");
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

  public async Task<List<UserRole>> GetAllUsers(Guid organisationId)
  {
    try
    {
      return await _userRepository.GetAllUsersWithRoles(organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to get users by organisation.", e);
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
      throw new Exception("Failed to get users by project.", e);
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


      var updatedUser = new User
      {
        Id = userId,
        Username = username,
        Credentials = new Credentials
        {
          PasswordHash = passwordHash,
          Salt = salt,
        }
      };

      return await _userRepository.UpdateUser(updatedUser);
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
}
