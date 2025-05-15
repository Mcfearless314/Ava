using Infrastructure.Models;

namespace Ava.API.DTOs;

public class UserRoleDto
{
  public Guid UserId { get; set; }
  public string Username { get; set; }
  public Roles Role { get; set; }
}
