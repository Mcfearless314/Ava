using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;
public class Credential
{
    [Key]
    public Guid UserId { get; set; }

    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public User User { get; set; } = null!;
}
