namespace Infrastructure.Models;

public class ProjectUser
{
    public required Guid UserId { get; set; }
    public required ProjectRoles Role { get; set; }
    public required Guid ProjectId { get; set; }
}

public enum ProjectRoles
{
  ProjectManager = 1,
  Contributor = 2
}

