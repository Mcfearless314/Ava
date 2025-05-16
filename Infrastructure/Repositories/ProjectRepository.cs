using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
  private readonly AppDbContext _context;

  public ProjectRepository(AppDbContext context)
  {
    _context = context;
  }


  public async Task<Project> CreateProject(string title, string subTitle, Guid organisationId, Guid projectManagerId)
  {
    var project = new Project
    {
      Id = Guid.NewGuid(),
      Title = title,
      Subtitle = subTitle,
      OrganisationId = organisationId,
      ProjectUsers = new List<ProjectUser>
      {
        new ()
        {
          UserId = projectManagerId,
          Role = ProjectRoles.ProjectManager,
          ProjectId = Guid.NewGuid()
        }
      }
    };

      await _context.Projects.AddAsync(project);
      await _context.SaveChangesAsync();
      return project;
  }

  public async Task<Project> UpdateProject(Guid projectId, string title, string subTitle, Guid userId)
  {
    var project = await _context.Projects.FindAsync(projectId);
    if (project == null) throw new Exception("Project not found");

    project.Title = title;
    project.Subtitle = subTitle;

    _context.Projects.Update(project);
    await _context.SaveChangesAsync();
    return project;
  }

  public async Task DeleteProject(Guid projectId, Guid userId)
  {
    var project = await _context.Projects.FindAsync(projectId);
    if (project == null) throw new Exception("Project not found");

    _context.Projects.Remove(project);
    await _context.SaveChangesAsync();
  }

  public async Task<List<ProjectTask>> GetProjectTasks(Guid projectId)
  {
    var project = await _context.Projects
      .Include(p => p.Tasks)
      .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) throw new Exception("Project not found");

    return project.Tasks.ToList();
  }

  public async Task<User> GetProjectManager(Guid projectId)
  {
      var project = await _context.Projects
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) throw new Exception("Project not found");

    var projectUser = await _context.ProjectUsers
      .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.Role == ProjectRoles.ProjectManager);

    var projectManager = await _context.Users
      .FirstOrDefaultAsync(u => u.Id == projectUser.UserId);

    if (projectManager == null) throw new Exception("Project manager not found");

    return projectManager;
  }

  public async Task RemoveUserFromProject(Guid userId, Guid projectId, Guid requesterId)
  {
    var project = await _context.Projects
      .Include(p => p.Tasks)
      .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) throw new Exception("Project not found");

    var user = await _context.Users
      .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null) throw new Exception("User not found");

    var projectUser = await _context.ProjectUsers
      .FirstOrDefaultAsync(pu => pu.UserId == userId && pu.ProjectId == projectId);

    if (projectUser != null)
    {
      _context.ProjectUsers.Remove(projectUser);
      await _context.SaveChangesAsync();
    }
  }
}
