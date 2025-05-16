using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;
using Service.Services.Security;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
  private readonly UserService _userService;
  private readonly JwtTokenService _jwtTokenService;

  public UserController(UserService userService, JwtTokenService jwtTokenService)
  {
    _userService = userService;
    _jwtTokenService = jwtTokenService;
  }

  [HttpGet("organisation/{organisationId}")]
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

  [HttpGet("project/{projectId}")]
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

  [HttpPost("update")]
  [Authorize]
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

  [HttpDelete("delete")]
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
}
