using Ava.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly UserService _userService;

  public UserController(UserService userService)
  {
    _userService = userService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] CredentialsDto dto)
  {
    try
    {
      await _userService.Register(dto.Username, dto.Password);
      return Ok(new { Message = "User registered successfully!" });
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
      await _userService.Authenticate(dto.Username, dto.Password);
      return Ok(new { Message = "User logged in successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("organisation/{organisationId}")]
  public async Task<IActionResult> GetAllUsers([FromRoute] Guid organisationId)
  {
    try
    {
      var users = await _userService.GetAllUsers(organisationId);
      return Ok(users);
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
      return Ok(users);
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
