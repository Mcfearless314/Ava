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

  public async Task<List<User>> GetAllUsers(Guid organisationId)
  {
    return await _context.Users
      .Where(ur => ur.OrganisationId == organisationId)
      .ToListAsync();
  }

  public async Task<List<User>> GetUsersByProject(Guid projectId)
  {
    var userIds = await _context.ProjectUsers
      .Where(pu => pu.ProjectId.ToString() == projectId.ToString())
      .Select(pu => pu.UserId)
      .ToListAsync();

    if (!userIds.Any()) throw new KeyNotFoundException();

    return await _context.Users
      .Where(u => userIds.Contains(u.Id))
      .ToListAsync();
  }

  public async Task<User> GetUserById(Guid userId)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    if (user == null) throw new KeyNotFoundException();
    return user;
  }

  public async Task<bool> GetUserByUsername(string username)
  {
    return await _context.Users
      .AnyAsync(u => u.Username == username);
  }

  public async Task<User?> GetUserWithCredentialsByUsername(string username)
  {
    return await _context.Users
      .Include(u => u.Credentials)
      .FirstOrDefaultAsync(u => u.Username == username);
  }

  public async Task<User> UpdateUser(Guid userId, string username, string salt, string passwordHash)
  {
    var existingUser = await _context.Users
      .Include(u => u.Credentials)
      .FirstOrDefaultAsync(u => u.Id == userId);

    if (existingUser == null) throw new KeyNotFoundException();

    existingUser.Username = username;
    existingUser.Credentials.PasswordHash = passwordHash;
    existingUser.Credentials.Salt = salt;

    await _context.SaveChangesAsync();

    return existingUser;
  }

  public Task DeleteUser(Guid userId)
  {
    var user = _context.Users.FirstOrDefault(u => u.Id == userId);
    if (user == null) throw new KeyNotFoundException();
    _context.Users.Remove(user);
    return _context.SaveChangesAsync();
  }

  public async Task<Guid?> GetOrganisationId(Guid userId)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    var organisationId = user?.OrganisationId;
    return organisationId;
  }
}
