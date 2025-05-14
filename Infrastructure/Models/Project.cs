using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Project
{
  [Key] public Guid Id { get; set; }
  public required string Title { get; set; }
  public string? Subtitle { get; set; }
  public Guid OrganisationId { get; set; }
  public Guid? ProjectManagerId { get; set; }
  public User? ProjectManager { get; set; }
  public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
