using Infrastructure.Models;

namespace Ava.API.DataTransferObjects;

public class AssignRoleToUserDto
{
  public Guid UserId { get; set; }
  public Roles Role { get; set; }
  public Guid OrganisationId { get; set; }
}
