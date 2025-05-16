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

  public async Task<string> AddUserToOrganisation(Guid userId, Guid organisationId)
  {
    try
    {
      var response = await _organizationRepository.AddUserToOrganisation(userId, organisationId);
      return $"User {response[0]} has been added to organisation {response[1]}";
    }
    catch (Exception e)
    {
      throw new Exception("Failed to add user to organisation.", e);
    }
  }

  public async Task<string> RemoveUserFromOrganisation(Guid userId, Guid organisationId)
  {
    try
    {
      var response = await _organizationRepository.RemoveUserFromOrganisation(userId, organisationId);
      return $"User {response[0]} has been removed from organisation {response[1]}";
    }
    catch (Exception e)
    {
      throw new Exception("Failed to remove user from organisation.", e);
    }
  }

  public async Task<User> GetAdminForOrganisation(Guid organisationId)
  {
    try
    {
      return await _organizationRepository.GetAdminForOrganisation(organisationId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to get admin for organisation.", e);
    }
  }
}
