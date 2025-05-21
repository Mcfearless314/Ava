using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Services.Application;
using Service.Services.Security;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly JwtTokenService _jwtTokenService;
  private readonly UserService _userService;

  public AuthController(JwtTokenService jwtTokenService, UserService userService)
  {
    _jwtTokenService = jwtTokenService;
    _userService = userService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] CredentialsDto dto)
  {

      await _userService.Register(dto.Username, dto.Password);
      return Ok(new { Message = $"{dto.Username} registered successfully!" });
  }

  [HttpPost("login")]
  [EnableRateLimiting("LoginLimiter")]
  public async Task<IActionResult> Login([FromBody] CredentialsDto dto)
  {

      var user = await _userService.Authenticate(dto.Username, dto.Password);
      var token = _jwtTokenService.GenerateJwtToken(dto.Username, user.Id);
      return Ok(new
      {
        Message = $"{dto.Username} logged in successfully!",
        Token = token
      });
  }
}
