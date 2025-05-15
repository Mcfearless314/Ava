using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _context;

  public UserRepository(AppDbContext context)
  {
    _context = context;
  }

  public Task Login(string username, string password)
  {
    throw new NotImplementedException();
  }

  public async Task Register(string username, string passwordHash, string salt)
  {
    var userId = Guid.NewGuid();

    var credentials = new Credentials
    {
      PasswordHash = passwordHash,
      Salt = salt,
      UserId = userId
    };

    var user = new User
    {
      Id = userId,
      Username = username,
      CreatedAt = DateTime.UtcNow,
      Credentials = credentials
    };

    await _context.Users.AddAsync(user);
    await _context.Credentials.AddAsync(credentials);
    await _context.SaveChangesAsync();
  }

  public Task<List<User>> GetAllUsers(Guid organisationId)
  {
    throw new NotImplementedException();
  }

  public Task<List<User>> GetUsersByProject(Guid projectId)
  {
    var userIds = _context.ProjectUsers
      .Where(x => x.ProjectId == projectId)
      .Select(x => x.UserId).ToListAsync();

    return _context.Users
      .Where(x => userIds.Result.Contains(x.Id))
      .ToListAsync();
  }

  public Task<User> GetUserById(Guid userId)
  {
    return _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
  }

  public Task<bool> GetUserByUsername(string username)
  {
    return _context.Users
      .AnyAsync(u => u.Username == username);
  }

  public Task<User?> GetUserWithCredentialsByUsername(string username)
  {
    return _context.Users
      .Include(u => u.Credentials)
      .FirstOrDefaultAsync(u => u.Username == username);
  }

  public Task<User> UpdateUser(User user)
  {
    throw new NotImplementedException();
  }

  public Task DeleteUser(Guid userId)
  {
    throw new NotImplementedException();
  }

  public Task AssignRoleToUser(Guid userId, Roles userRole, Guid organisationId)
  {
    throw new NotImplementedException();
  }
}
