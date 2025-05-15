using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Organisation
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public ICollection<Project> Projects { get; set; } = new List<Project>();
  public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
