using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Project
{
  [Key] public Guid Id { get; set; }
  public required string Title { get; set; }
  public string? Subtitle { get; set; }
  public Guid OrganisationId { get; set; }
  public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
  public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
}
