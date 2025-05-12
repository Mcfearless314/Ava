using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IProjectRepository

{
  public Task<Project> CreateProject(string title, string subTitle, Guid organisationId);

  public Task<Project> UpdateProject(Guid projectId, string title, string subTitle);

  public Task DeleteProject(Guid projectId);

  public Task<List<ProjectTask>> GetProjectTasks(Guid projectId);

  public Task<User> GetProjectManager(Guid projectId);

  public Task RemoveUserFromProject(Guid userId, Guid projectId);
}
