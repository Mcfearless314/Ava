using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;
using Service.Services.Security;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly UserService _userService;
  private readonly JwtTokenService _jwtTokenService;

  public UserController(UserService userService, JwtTokenService jwtTokenService)
  {
    _userService = userService;
    _jwtTokenService = jwtTokenService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] CredentialsDto dto)
  {
    try
    {
      await _userService.Register(dto.Username, dto.Password);
      return Ok(new { Message = $"{dto.Username} registered successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] CredentialsDto dto)
  {
    try
    {
      var user = await _userService.Authenticate(dto.Username, dto.Password);
      var token = _jwtTokenService.GenerateJwtToken(dto.Username, user.Id);
      return Ok(new
      {
        Message = $"{dto.Username} logged in successfully!",
        Token = token
      });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("organisation/{organisationId}")]
  public async Task<ActionResult<List<UserDto>>> GetUsers(Guid organisationId)
  {
    try
    {
      var users = await _userService.GetAllUsers(organisationId);

      var dtos = users.Select(ur => new UserDto
      {
        Id = ur.Id,
        Username = ur.Username,
      }).ToList();

      return Ok(dtos);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("project/{projectId}")]
  public async Task<IActionResult> GetUsersByProject([FromRoute] Guid projectId)
  {
    try
    {
      var users = await _userService.GetUsersByProject(projectId);

      var userDtos = users.Select(u => new UserDto
      {
        Id = u.Id,
        Username = u.Username
      }).ToList();

      return Ok(userDtos);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPost("update/{userId}")]
  public async Task<IActionResult> Update([FromBody] CredentialsDto dto, [FromRoute] Guid userId)
  {
    try
    {
      await _userService.UpdateUser(userId, dto.Username, dto.Password);
      return Ok(new { Message = "User updated successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("delete/{userId}")]
  public async Task<IActionResult> Delete([FromRoute] Guid userId)
  {
    try
    {
      await _userService.DeleteUser(userId);
      return Ok(new { Message = "User deleted successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }
}
