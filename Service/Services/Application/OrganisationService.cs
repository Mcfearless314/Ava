using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Services.Application;

public class OrganisationService
{
  private readonly IOrganizationRepository _organizationRepository;

  public OrganisationService(IOrganizationRepository organizationRepository)
  {
    _organizationRepository = organizationRepository;
  }

  public async Task<Organisation> CreateOrganisation(string name, Guid userId)
  {
    try
    {
      return await _organizationRepository.CreateOrganisation(name, userId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to create organisation.", e);
    }
  }

  public async Task<Organisation> UpdateOrganisation(Guid organisationId, string name)
  {
    try
    {
      return await _organizationRepository.UpdateOrganisation(organisationId, name);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to update organisation.", e);
    }
  }

  public async Task DeleteOrganisation(Guid organisationId)
  {
    try
    {
      await _organizationRepository.DeleteOrganisation(organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to delete organisation.", e);
    }
  }

  public async Task AddUserToOrganisation(Guid userId, Guid organisationId)
  {
    try
    {
      await _organizationRepository.AddUserToOrganisation(userId, organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to add user to organisation.", e);
    }
  }

  public async Task RemoveUserFromOrganisation(Guid userId, Guid organisationId)
  {
    try
    {
      await _organizationRepository.RemoveUserFromOrganisation(userId, organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to remove user from organisation.", e);
    }
  }

  public async Task AssignRoleToUser(Guid userId, Roles userRole, Guid organisationId)
  {
    try
    {
      await _organizationRepository.AssignRoleToUser(userId, userRole, organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to assign role to user.", e);
    }
  }
}
