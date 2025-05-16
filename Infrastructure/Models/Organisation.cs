using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Organisation
{
  [Key]
  public Guid Id { get; set; }
  public required string Name { get; set; }
  public User AdminUser { get; set; }
  public Guid AdminUserId { get; set; }
  public ICollection<Project> Projects { get; set; } = new List<Project>();
  public ICollection<User> Users { get; set; } = new List<User>();
}
