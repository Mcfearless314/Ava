using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Credentials Credentials { get; set; }
}
