namespace Infrastructure.Models;

public class UserRole
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Guid OrganisationId { get; set; }
    public Roles Role { get; set; }
}

public enum Roles
{
    Admin = 1,
    User = 2
}
