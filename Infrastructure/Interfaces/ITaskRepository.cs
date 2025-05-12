using Infrastructure.Models;
using TaskStatus = Infrastructure.Models.TaskStatus;

namespace Infrastructure.Interfaces;

public interface ITaskRepository
{
  public Task<ProjectTask> CreateTask(string title, string body, DateTime createdAt, TaskStatus status, Guid projectId);

  public Task<ProjectTask> UpdateTask(int taskId, string title, string body);

  public Task DeleteTask(int taskId);

  public Task<ProjectTask> GetTask(int taskId);

  public Task UpdateTaskStatus(int taskId, TaskStatus status);


}
