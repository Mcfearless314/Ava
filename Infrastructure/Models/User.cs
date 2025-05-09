using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public Credentials Credentials { get; set; }
}
