using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Services.Application;

public class ProjectService
{
  private readonly IProjectRepository _projectRepository;
  private readonly UserService _userService;

  public ProjectService(IProjectRepository projectRepository, UserService userService)
  {
    _projectRepository = projectRepository;
    _userService = userService;
  }

  public async Task<Project> CreateProject(string title, string subTitle, Guid organisationId, Guid projectManagerId)
  {
    try
    {
      var users = await _userService.GetAllUsers(organisationId);
      if (!users.Select(u => u.Id).Contains(projectManagerId))
      {
        throw new Exception("Project manager not found in the organisation.");
      }

      return await _projectRepository.CreateProject(title, subTitle, organisationId, projectManagerId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to create project.", e);
    }
  }

  public async Task<Project> UpdateProject(Guid projectId, string title, string subTitle, Guid userId)
  {
    try
    {
      return await _projectRepository.UpdateProject(projectId, title, subTitle, userId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to update project.", e);
    }
  }

  public async Task DeleteProject(Guid projectId, Guid userId)
  {
    try
    {
      await _projectRepository.DeleteProject(projectId, userId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to delete project.", e);
    }
  }

  public async Task<List<ProjectTask>> GetProjectTasks(Guid projectId)
  {
    try
    {
      return await _projectRepository.GetProjectTasks(projectId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to get project tasks.", e);
    }
  }

  public async Task<User> GetProjectManager(Guid projectId)
  {
    try
    {
      return await _projectRepository.GetProjectManager(projectId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to get project manager.", e);
    }
  }

  public async Task RemoveUserFromProject(Guid userId, Guid projectId, Guid requesterId)
  {
    try
    {
      await _projectRepository.RemoveUserFromProject(userId, projectId, requesterId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to remove user from project.", e);
    }
  }

  public async Task<Project> GetProjectByProjectId(Guid projectId)
  {
    try
    {
      return await _projectRepository.GetProjectById(projectId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to retrieve project");
    }
  }

  public async Task<object> CheckUserAccessToProject(Guid projectId, Guid userId)
  {
    var users = await _userService.GetUsersByProject(projectId);
    return users.Select(u => u.Id).Contains(userId) ? new { HasAccess = true } : new { HasAccess = false };
  }

  public async Task<User> AddUserToProject(Guid userId, Guid projectId)
  {
    try
    {
      var users = await _userService.GetUsersByProject(projectId);
      if (users.Select(u => u.Id).Contains(userId))
      {
        throw new Exception("User already exists in the project.");
      }
      return await _projectRepository.AddUserToProject(userId, projectId);
    }
    catch (Exception e)
    {
      throw new Exception("Failed to add user to project.", e);
    }
  }
}
