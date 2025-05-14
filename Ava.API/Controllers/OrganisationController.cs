using Ava.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganisationController : ControllerBase
{
  private readonly OrganisationService _organisationService;

  public OrganisationController(OrganisationService organisationService)
  {
    _organisationService = organisationService;
  }

  [HttpPost("create")]
  public async Task<IActionResult> CreateOrganisation([FromBody] CreateOrganisationDTO dto)
  {
    try
    {
      var organisation = await _organisationService.CreateOrganisation(dto.Name, dto.UserId);
      return Ok(organisation);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPut("update")]
  public async Task<IActionResult> UpdateOrganisation([FromBody] UpdateOrganisationDTO dto)
  {
    try
    {
      var organisation = await _organisationService.UpdateOrganisation(dto.OrganisationId, dto.Name);
      return Ok(organisation);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("delete/{organisationId}")]
  public async Task<IActionResult> DeleteOrganisation(Guid organisationId)
  {
    try
    {
      await _organisationService.DeleteOrganisation(organisationId);
      return NoContent();
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPost("addUser")]
  public async Task<IActionResult> AddUserToOrganisation([FromBody] AddOrRemoveUserFromOrganisationDTO dto)
  {
    try
    {
      await _organisationService.AddUserToOrganisation(dto.UserId, dto.OrganisationId);
      return NoContent();
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("removeUser")]
  public async Task<IActionResult> RemoveUserFromOrganisation([FromBody] AddOrRemoveUserFromOrganisationDTO dto)
  {
    try
    {
      await _organisationService.RemoveUserFromOrganisation(dto.UserId, dto.OrganisationId);
      return NoContent();
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }
}
