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
public class ProjectTaskController : ControllerBase
{
  private readonly ProjectTaskService _projectTaskService;

  public ProjectTaskController(ProjectTaskService projectTaskService)
  {
    _projectTaskService = projectTaskService;
  }

  [Authorize(Policy = "MustBeAdminOrProjectManager")]
  [HttpPost("create")]
  public async Task<IActionResult> CreateProjectTask([FromBody] CreateOrUpdateProjectTaskDto dto)
  {
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var projectTask = await _projectTaskService.CreateProjectTask(dto.Title, dto.Body, dto.Status, dto.ProjectId, userId);
    return Ok(projectTask);
  }

  [Authorize(Policy = "MustBeAdminOrProjectManager")]
  [HttpPut("update/{projectTaskId}")]
  public async Task<IActionResult> UpdateProjectTask(string projectTaskId, [FromBody] CreateOrUpdateProjectTaskDto dto)
  {
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var projectTask =
      await _projectTaskService.UpdateProjectTask(projectTaskId, dto.Title, dto.Body, dto.ProjectId, userId);
    if (projectTask == null)
      return NotFound("No Project Task Found");
    return Ok(projectTask);
  }

  [Authorize(Policy = "MustBeAdminOrProjectManager")]
  [HttpDelete("delete/{projectTaskId}/{projectId:guid}")]
  public async Task<IActionResult> DeleteProjectTask(string projectTaskId, Guid projectId)
  {
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var projectTask = await _projectTaskService.DeleteProjectTask(projectTaskId, projectId, userId);
    if (projectTask == null)
      return NotFound("No Project Task Found");
    return Ok("Project Task Deleted");
  }

  [Authorize(Policy = "MustBeAdminOrProjectUser")]
  [HttpGet("get/{projectTaskId}/{projectId:guid}")]
  public async Task<IActionResult> GetProjectTask(string projectTaskId, Guid projectId)
  {
    var projectTask = await _projectTaskService.GetProjectTask(projectTaskId, projectId);
    if (projectTask == null)
      return NotFound("No Project Task Found");
    return Ok(projectTask);
  }

  [Authorize(Policy = "MustBeAdminOrProjectUser")]
  [HttpPut("updateStatus/{projectTaskId}/{projectTaskStatus}/{projectId:guid}")]
  public async Task<IActionResult> UpdateProjectTaskStatus(string projectTaskId, ProjectTaskStatus projectTaskStatus,
    Guid projectId)
  {
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var projectTask = await _projectTaskService.UpdateProjectTaskStatus(projectTaskId, projectTaskStatus, projectId, userId);
    if (projectTask == null)
      return NotFound("No Project Task Found");
    return Ok(projectTask);
  }

  [Authorize(Policy = "MustBeAdminOrProjectUser")]
  [HttpGet("getAllProjectTasksForProject/{projectId:guid}")]
  public async Task<IActionResult> GetAllProjectTasksForProject(Guid projectId)
  {
    var projectTasks = await _projectTaskService.GetAllProjectTasksForProject(projectId);
    if (projectTasks.Count == 0)
      return NotFound("No Project Tasks Found");
    return Ok(projectTasks);
  }
}
