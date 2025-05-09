using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Project
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Subtitle { get; set; }

    public Guid OrganisationId { get; set; }
    public Organisation Organisation { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    public ICollection<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
}
