using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.Services.Utility;

namespace Service.Services.Application;

public class ProjectTaskService
{
  private readonly IProjectTaskRepository _projectTaskRepository;

  public ProjectTaskService(IProjectTaskRepository projectTaskRepository)
  {
    _projectTaskRepository = projectTaskRepository;
  }

  public async Task<ProjectTask?> CreateProjectTask(string dtoTitle, string? dtoBody, ProjectTaskStatus dtoStatus,
    Guid dtoProjectId)
  {
    var id = await GenerateUniqueProjectTaskIdAsync(dtoProjectId);
    var createdTime = DateTime.UtcNow;
    return await _projectTaskRepository.CreateProjectTask(id, dtoTitle, dtoBody, createdTime,
      ProjectTaskStatus.ToDo, dtoProjectId);
  }

  public async Task<object?> UpdateProjectTask(string projectTaskId, string dtoTitle, string? dtoBody,
    ProjectTaskStatus dtoStatus,
    Guid projectId)
  {
    var projectTask = await _projectTaskRepository.GetProjectTask(projectTaskId, projectId);
    if (projectTask == null)
      return null;

    projectTask.Title = dtoTitle;
    projectTask.Body = dtoBody;
    projectTask.Status = dtoStatus;

    await _projectTaskRepository.UpdateProjectTask(projectTaskId, dtoTitle, dtoBody);
    return projectTask;
  }

  public async Task<ProjectTask?> DeleteProjectTask(string projectTaskId, Guid projectId)
  {
    var projectTask = await _projectTaskRepository.GetProjectTask(projectTaskId, projectId);
    if (projectTask == null)
      return projectTask;

    await _projectTaskRepository.DeleteProjectTask(projectTaskId);
    return projectTask;
  }

  public async Task<ProjectTask?> GetProjectTask(string projectTaskId, Guid projectId)
  {
    var projectTask = await _projectTaskRepository.GetProjectTask(projectTaskId, projectId);
    if (projectTask == null)
      return null;

    return projectTask;
  }

  public async Task<object?> UpdateProjectTaskStatus(string projectTaskId, ProjectTaskStatus status, Guid projectId)
  {
    var projectTask = await _projectTaskRepository.GetProjectTask(projectTaskId, projectId);
    if (projectTask == null)
      return null;

    projectTask.Status = status;
    await _projectTaskRepository.UpdateProjectTaskStatus(projectTaskId, status, projectId);
    return projectTask;
  }

  public async Task<List<ProjectTask>> GetAllProjectTasksForProject(Guid projectId)
  {
    var projectTasks = await _projectTaskRepository.GetAllProjectTasksForProject(projectId);
    return projectTasks;
  }

  private async Task<string> GenerateUniqueProjectTaskIdAsync(Guid projectId)
  {
    const int maxAttempts = 1000;
    var existingIds = await _projectTaskRepository
      .GetProjectTaskIdsForProject(projectId);

    for (int i = 0; i < maxAttempts; i++)
    {
      var newId = GenerateRandomProjectTaskId.GenerateRandomId();
      if (!existingIds.Contains(newId))
        return newId;
    }

    throw new Exception("Unable to generate unique task ID after multiple attempts.");
  }
}
