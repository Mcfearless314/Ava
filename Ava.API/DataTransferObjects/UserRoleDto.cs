using Infrastructure.Models;

namespace Ava.API.DataTransferObjects;

public class UserRoleDto
{
  public Guid UserId { get; set; }
  public string Username { get; set; }
  public Roles Role { get; set; }
}
