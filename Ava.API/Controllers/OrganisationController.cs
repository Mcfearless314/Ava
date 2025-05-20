using System.Data;
using System.Security.Claims;
using Ava.API.DataTransferObjects;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrganisationController : ControllerBase
{
  private readonly OrganisationService _organisationService;

  public OrganisationController(OrganisationService organisationService)
  {
    _organisationService = organisationService;
  }

  [HttpPost("create")]
  public async Task<IActionResult> CreateOrganisation([FromBody] CreateOrganisationDto organisationDto)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
    {
      return Unauthorized();
    }
    var organisation = await _organisationService.CreateOrganisation(organisationDto.Name, userId);
    return Ok(new
      { organisationId = organisation.Id, message = $"Organisation {organisation.Name} created successfully!" });
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpPut("update")]
  public async Task<IActionResult> UpdateOrganisation([FromBody] UpdateOrganisationDto dto)
  {
    var organisation = await _organisationService.UpdateOrganisation(dto.OrganisationId, dto.Name);
    return Ok($"Organisation {organisation.Name} updated successfully!");
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpDelete("delete/{organisationId}")]
  public async Task<IActionResult> DeleteOrganisation(Guid organisationId)
  {
    await _organisationService.DeleteOrganisation(organisationId);
    return Ok("Organisation deleted successfully!");
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpPost("addUser")]
  public async Task<IActionResult> AddUserToOrganisation([FromBody] AddOrRemoveUserFromOrganisationDto dto)
  {
    var user = await _organisationService.AddUserToOrganisation(dto.Username, dto.OrganisationId);
    var userDto = new UserDto
    {
      Id = user.userId,
      Username = user.username,
    };
    return Ok(userDto);
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpDelete("removeUser")]
  public async Task<IActionResult> RemoveUserFromOrganisation([FromBody] AddOrRemoveUserFromOrganisationDto dto)
  {
    return Ok(await _organisationService.RemoveUserFromOrganisation(dto.Username, dto.OrganisationId));
  }

  [Authorize(Policy = "MustBePartOfOrganisation")]
  [HttpGet("getAllProjects/{organisationId:guid}")]
  public async Task<IActionResult> GetAllProjects(Guid organisationId)
  {
    var projects = await _organisationService.GetAllProjects(organisationId);
    var projectDtos = projects.Select(project => new ProjectDto()
      { ProjectId = project.Id, Title = project.Title, SubTitle = project.Subtitle, }).ToList();

    return Ok(projectDtos);
  }
}
