using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IUserRepository
{
  public Task Login(string username, string password);

  public Task Register(string username, string passwordHash, string salt);

  public Task<List<User>> GetAllUsers(Guid organisationId);

  public Task<List<User>> GetUsersByProject(Guid projectId);

  public Task<User> GetUserById(Guid userId);

  public Task<bool> GetUserByUsername(string username);

  public Task<User?> GetUserWithCredentialsByUsername(string username);

  public Task<User> UpdateUser(User user);

  public Task DeleteUser(Guid userId);

  public Task AssignRoleToUser(Guid userId, Roles userRole, Guid organisationId);

}
