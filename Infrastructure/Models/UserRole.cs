namespace Infrastructure.Models;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid OrganisationId { get; set; }
    public Roles Role { get; set; }
}

public enum Roles
{
    Admin = 1,
    User = 2
}
