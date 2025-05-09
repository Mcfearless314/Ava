namespace Infrastructure.Models;

public class UserRole
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid OrganisationId { get; set; }
    public Organisation Organisation { get; set; } = null!;

    public int Role { get; set; }
}