using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectTaskRepository : IProjectTaskRepository
{
  private readonly AppDbContext _context;

  public ProjectTaskRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<ProjectTask> CreateProjectTask(string id, string title, string? body, DateTime createdAt,
    ProjectTaskStatus status, Guid projectId)
  {
    var projectTask = new ProjectTask
    {
      Id = id,
      Title = title,
      Body = body,
      CreatedAt = createdAt,
      Status = status,
      ProjectId = projectId
    };

    await _context.ProjectTasks.AddAsync(projectTask);
    await _context.SaveChangesAsync();

    return projectTask;
  }

  public async Task<ProjectTask?> UpdateProjectTask(string projectTaskId, string title, string? body)
  {
    var projectTask = await _context.ProjectTasks
      .FirstOrDefaultAsync(task => task.Id == projectTaskId);

    if (projectTask == null) return null;

    projectTask.Title = title;
    projectTask.Body = body;

    await _context.SaveChangesAsync();

    return projectTask;
  }

  public async Task<ProjectTask?> DeleteProjectTask(string projectTaskId)
  {
    var projectTask = await _context.ProjectTasks
      .FirstOrDefaultAsync(task => task.Id == projectTaskId);

    if (projectTask == null) return null;

    projectTask.Status = ProjectTaskStatus.Deleted;
    _context.ProjectTasks.Update(projectTask);
    await _context.SaveChangesAsync();

    return projectTask;
  }

  public async Task<ProjectTask?> GetProjectTask(string projectTaskId, Guid projectId)
  {
    var projectTask = await _context.ProjectTasks
      .FirstOrDefaultAsync(task => task.Id == projectTaskId && task.ProjectId == projectId);

    return projectTask;
  }

  public async Task<ProjectTask?> UpdateProjectTaskStatus(string projectTaskId, ProjectTaskStatus status,
    Guid projectId)
  {
    var projectTask = await _context.ProjectTasks
      .FirstOrDefaultAsync(task => task.Id == projectTaskId && task.ProjectId == projectId);

    if (projectTask == null) return projectTask;
    projectTask.Status = status;
    await _context.SaveChangesAsync();

    return projectTask;
  }

  public async Task<List<ProjectTask>> GetAllProjectTasksForProject(Guid projectId)
  {
    var projectTasks = await _context.ProjectTasks
      .Where(task => task.ProjectId == projectId)
      .ToListAsync();

    return projectTasks;
  }

  public async Task<List<string>> GetProjectTaskIdsForProject(Guid projectId)
  {
    var projectTaskIds = await _context.ProjectTasks
      .Where(task => task.ProjectId == projectId)
      .Select(task => task.Id)
      .ToListAsync();

    return projectTaskIds;
  }
}
