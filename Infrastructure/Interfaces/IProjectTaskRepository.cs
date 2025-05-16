using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IProjectTaskRepository
{
  public Task<ProjectTask> CreateProjectTask(string id, string title, string? body, DateTime createdAt, ProjectTaskStatus status, Guid projectId);

  public Task<ProjectTask?> UpdateProjectTask(string projectTaskId, string title, string? body);

  public Task<ProjectTask?> DeleteProjectTask(string projectTaskId);

  public Task<ProjectTask?> GetProjectTask(string projectTaskId, Guid projectId);

  public Task<ProjectTask?> UpdateProjectTaskStatus(string projectTaskId, ProjectTaskStatus status, Guid projectId);
  public Task<List<ProjectTask>> GetAllProjectTasksForProject(Guid projectId);
  Task<List<string>> GetProjectTaskIdsForProject(Guid projectId);

}
