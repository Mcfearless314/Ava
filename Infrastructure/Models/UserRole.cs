namespace Infrastructure.Models;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid OrganisationId { get; set; }
    public int Role { get; set; }
}
