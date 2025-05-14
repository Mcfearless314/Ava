using Ava.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
  private readonly ProjectService _projectService;

  public ProjectController(ProjectService projectService)
  {
    _projectService = projectService;
  }

  [HttpPost("create")]
  public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
  {
    try
    {
      var project = await _projectService.CreateProject(dto.Title, dto.SubTitle, dto.OrganisationId, dto.ProjectManagerId);
      return Ok(project);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPut("update/{projectId}")]
  public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] CreateProjectDto dto)
  {
    try
    {
      var project = await _projectService.UpdateProject(projectId, dto.Title, dto.SubTitle, Guid.NewGuid());
      return Ok(project);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("delete/{projectId}")]
  public async Task<IActionResult> DeleteProject(Guid projectId)
  {
    try
    {
      await _projectService.DeleteProject(projectId, Guid.NewGuid());
      return Ok(new { Message = "Project deleted successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("tasks/{projectId}")]
  public async Task<IActionResult> GetProjectTasks(Guid projectId)
  {
    try
    {
      var tasks = await _projectService.GetProjectTasks(projectId);
      return Ok(tasks);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("manager/{projectId}")]
  public async Task<IActionResult> GetProjectManager(Guid projectId)
  {
    try
    {
      var manager = await _projectService.GetProjectManager(projectId);
      return Ok(manager);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("remove-user/{userId}/{projectId}")]
  public async Task<IActionResult> RemoveUserFromProject(Guid userId, Guid projectId)
  {
    try
    {
      await _projectService.RemoveUserFromProject(userId, projectId, Guid.NewGuid());
      return Ok(new { Message = "User removed from project successfully!" });
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

}
