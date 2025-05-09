using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;
public class Credentials
{
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
}
