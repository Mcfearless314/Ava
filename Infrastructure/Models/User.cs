using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public Credential Credential { get; set; }
    public ICollection<UserRole> Roles { get; set; }
    public ICollection<ProjectUser> Projects { get; set; }
    public ICollection<ProjectManager> ManagedProjects { get; set; }
}
