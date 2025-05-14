using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IProjectRepository

{
  public Task<Project> CreateProject(string title, string subTitle, Guid organisationId, Guid projectManagerId);

  public Task<Project> UpdateProject(Guid projectId, string title, string subTitle, Guid userId);

  public Task DeleteProject(Guid projectId, Guid userId);

  public Task<List<ProjectTask>> GetProjectTasks(Guid projectId);

  public Task<User> GetProjectManager(Guid projectId);

  public Task RemoveUserFromProject(Guid userId, Guid projectId, Guid requesterId);
}
