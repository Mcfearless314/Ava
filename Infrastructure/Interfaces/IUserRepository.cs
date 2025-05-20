using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IUserRepository
{
  public Task Register(string username, string passwordHash, string salt);

  public Task<List<User>> GetAllUsers(Guid organisationId);

  public Task<List<User>> GetUsersByProject(Guid projectId);

  public Task<User> GetUserById(Guid userId);

  public Task<bool> GetUserByUsername(string username);

  public Task<User?> GetUserWithCredentialsByUsername(string username);

  public Task<User> UpdateUser(Guid userId, string username, string salt, string passwordHash);

  public Task DeleteUser(Guid userId);

  Task<Guid?> GetOrganisationId(Guid userId);
}
