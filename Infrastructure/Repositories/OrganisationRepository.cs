using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrganisationRepository : IOrganizationRepository
{
  private readonly AppDbContext _context;

  public OrganisationRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<Organisation> CreateOrganisation(string name, Guid userId)
  {
    var organisation = new Organisation
    {
      Name = name,
      Id = Guid.NewGuid(),
      AdminUserId = userId,
    };

    await _context.Organisations.AddAsync(organisation);
    await _context.SaveChangesAsync();
    return organisation;
  }

  public async Task<Organisation> UpdateOrganisation(Guid organisationId, string name)
  {
    var organisation = await _context.Organisations
      .FirstOrDefaultAsync(o => o.Id == organisationId);
    if (organisation == null)
    {
      throw new Exception("Organisation not found");
    }

    organisation.Name = name;
    await _context.SaveChangesAsync();
    return organisation;
  }

  public async Task DeleteOrganisation(Guid organisationId)
  {
    var organisation = await _context.Organisations
      .FirstOrDefaultAsync(o => o.Id == organisationId);
    if (organisation == null)
    {
      throw new Exception("Organisation not found");
    }

    _context.Organisations.Remove(organisation);
    await _context.SaveChangesAsync();
  }

  public async Task<string[]> AddUserToOrganisation(Guid userId, Guid organisationId)
  {
    var user = await _context.Users
      .FirstOrDefaultAsync(u => u.Id == userId);
    if (user == null)
    {
      throw new Exception("User not found");
    }

    var organisation = await _context.Organisations
      .FirstOrDefaultAsync(o => o.Id == organisationId);
    if (organisation == null)
    {
      throw new Exception("Organisation not found");
    }

    user.OrganisationId = organisationId;
    await _context.SaveChangesAsync();
    return [user.Username, organisation.Name];
  }

  public async Task<string[]> RemoveUserFromOrganisation(Guid userId, Guid organisationId)
  {
    var user = await _context.Users
      .FirstOrDefaultAsync(u => u.Id == userId);
    if (user == null)
    {
      throw new Exception("User not found");
    }

    var organisation = await _context.Organisations
      .FirstOrDefaultAsync(o => o.Id == organisationId);
    if (organisation == null)
    {
      throw new Exception("Organisation not found");
    }

    user.OrganisationId = Guid.Empty;

    await _context.SaveChangesAsync();
    return [user.Username, organisation.Name];
  }

  public async Task<User> GetAdminForOrganisation(Guid organisationId)
  {
    var organisation = await _context.Organisations
      .Include(o => o.AdminUser)
      .FirstOrDefaultAsync(o => o.Id == organisationId);
    if (organisation == null)
    {
      throw new Exception("Organisation not found");
    }

    if(organisation.AdminUser == null)
    {
      throw new Exception("Admin user not found");
    }
    return organisation.AdminUser;
  }
}
