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
  public async Task<IActionResult> Register([FromBody] LoginDto dto)
  {
    try
    {
      await _userService.Register(dto.Username, dto.Password);
      return Ok(new { Message = "User registered successfully!" });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(); //TODO
    }
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDto dto)
  {
    try
    {
      await _userService.Authenticate(dto.Username, dto.Password);
      return Ok(new { Message = "User logged in successfully!" });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(); //TODO
    }
  }
}
