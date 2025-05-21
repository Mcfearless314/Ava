using System.Security.Claims;
using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
  private readonly UserService _userService;
  public UserController(UserService userService)
  {
    _userService = userService;
  }

  [Authorize(Policy = "MustBePartOfOrganisation")]
  [HttpGet("getUsers/{organisationId}")]
  public async Task<ActionResult<List<UserDto>>> GetUsers(Guid organisationId)
  {
    var users = await _userService.GetAllUsers(organisationId);

    var dtos = users.Select(ur => new UserDto
    {
      Id = ur.Id,
      Username = ur.Username,
    }).ToList();

    return Ok(dtos);
  }

  [Authorize(Policy = "MustBeAdminOrProjectUser")]
  [HttpGet("getUsersByProject/{projectId}")]
  public async Task<IActionResult> GetUsersByProject([FromRoute] Guid projectId)
  {
    var users = await _userService.GetUsersByProject(projectId);

    var userDtos = users.Select(u => new UserDto
    {
      Id = u.Id,
      Username = u.Username
    }).ToList();

    return Ok(userDtos);
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpPost("updateUser")]
  public async Task<IActionResult> Update([FromBody] CredentialsDto dto)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
    {
      return Unauthorized();
    }

    await _userService.UpdateUser(userId, dto.Username, dto.Password);
    return Ok(new { Message = "User updated successfully!" });
  }

  [HttpDelete("deleteUser")]
  public async Task<IActionResult> Delete()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
    {
      return Unauthorized();
    }

    await _userService.DeleteUser(userId);
    return Ok(new { Message = "User deleted successfully!" });
  }

  [HttpGet("getOrganisationId/{userId:guid}")]
  public async Task<IActionResult> GetOrganisationId([FromRoute] Guid userId)
  {
    var organisationId = await _userService.GetOrganisationId(userId);

    return Ok(new { OrganisationId = organisationId });
  }
}
