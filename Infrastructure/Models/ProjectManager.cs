namespace Infrastructure.Models;

public class ProjectManager
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}