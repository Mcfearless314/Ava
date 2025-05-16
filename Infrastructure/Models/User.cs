using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    [Key]
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Credentials Credentials { get; set; }
    public Guid? OrganisationId { get; set; }
}
