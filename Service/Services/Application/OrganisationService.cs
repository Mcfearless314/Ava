using System.Data;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Services.Application;

public class OrganisationService
{
  private readonly IOrganizationRepository _organizationRepository;
  private readonly UserService _userService;

  public OrganisationService(IOrganizationRepository organizationRepository, UserService userService)
  {
    _organizationRepository = organizationRepository;
    _userService = userService;
  }

  public async Task<Organisation> CreateOrganisation(string name, Guid userId)
  {
    try
    {
      return await _organizationRepository.CreateOrganisation(name, userId);
    }
    catch (DuplicateNameException e)
    {
      throw new DuplicateNameException("Organisation with this name already exists");
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
      throw new Exception("Failed to update organisation.");
    }
  }

  public async Task DeleteOrganisation(Guid organisationId)
  {
    try
    {
      await _organizationRepository.DeleteOrganisation(organisationId);
    }
    catch (Exception)
    {
      throw new Exception("Failed to delete organisation.");
    }
  }

  public async Task<(string username, Guid userId)> AddUserToOrganisation(string username, Guid organisationId)
  {
    var user = await _userService.GetUserByUsername(username);
    if (user == null)
    {
      throw new KeyNotFoundException("User not found");
    }

    try
    {
      return await _organizationRepository.AddUserToOrganisation(user.Id, organisationId);
    }
    catch (Exception e)
    {
      if (e is KeyNotFoundException)
      {
        throw new KeyNotFoundException("User not found");
      }
      throw new Exception(e.Message);
    }
  }

  public async Task<string> RemoveUserFromOrganisation(string username, Guid organisationId)
  {
    var user = await _userService.GetUserByUsername(username);
    if (user == null)
    {
      throw new KeyNotFoundException("User not found");
    }

    try
    {
      var response = await _organizationRepository.RemoveUserFromOrganisation(user.Id, organisationId);
      return $"User {response[0]} has been removed from organisation {response[1]}";
    }
    catch (Exception)
    {
      throw new Exception("Failed to remove user from organisation.");
    }
  }

  public async Task<User> GetAdminForOrganisation(Guid organisationId)
  {
    try
    {
      return await _organizationRepository.GetAdminForOrganisation(organisationId);
    }
    catch (Exception)
    {
      throw new Exception("Failed to get admin for organisation.");
    }
  }

  public async Task<ICollection<Project>> GetAllProjects(Guid organisationId)
  {
    try
    {
      return await _organizationRepository.GetAllProjects(organisationId);
    }
    catch (Exception)
    {
      throw new Exception("Failed to get all projects for organisation.");
    }
  }

  public async Task<Guid> GetOrganisationByProject(Guid projectId)
  {
    try
    {
      return await _organizationRepository.GetOrganisationByProject(projectId);
    }
    catch (Exception e)
    {
      if (e is KeyNotFoundException)
      {
        throw new KeyNotFoundException("Organisation Not Found");
      }
      throw new Exception("Failed to get organisation.");
    }
  }
}
