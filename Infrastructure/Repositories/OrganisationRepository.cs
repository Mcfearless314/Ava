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
    };

    var userRole = new UserRole
    {
      UserId = userId,
      OrganisationId = organisation.Id,
      Role = Roles.Admin
    };

    await _context.UserRoles.AddAsync(userRole);
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

  public async Task AddUserToOrganisation(Guid userId, Guid organisationId)
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

    var userRole = new UserRole
    {
      UserId = userId,
      OrganisationId = organisationId,
      Role = Roles.User
    };

    await _context.UserRoles.AddAsync(userRole);
    await _context.SaveChangesAsync();
  }

  public async Task RemoveUserFromOrganisation(Guid userId, Guid organisationId)
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

    var userRole = await _context.UserRoles
      .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.OrganisationId == organisationId);
    if (userRole == null)
    {
      throw new Exception("User is not part of the organisation");
    }

    _context.UserRoles.Remove(userRole);
    await _context.SaveChangesAsync();
  }

  public async Task AssignRoleToUser(Guid userId, Roles userRole, Guid organisationId)
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

    var existingUserRole = await _context.UserRoles
      .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.OrganisationId == organisationId);
    if (existingUserRole != null)
    {
      existingUserRole.Role = userRole;
      await _context.SaveChangesAsync();
    }
    else
    {
      throw new Exception("User role not found");
    }
  }
}
