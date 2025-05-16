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

  [HttpPost("create")]
  public async Task<IActionResult> CreateProjectTask([FromBody] CreateOrUpdateProjectTaskDto dto)
  {
    try
    {
      var projectTask = await _projectTaskService.CreateProjectTask(dto.Title, dto.Body, dto.Status, dto.ProjectId);
      return Ok(projectTask);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPut("update/{projectTaskId}")]
  public async Task<IActionResult> UpdateProjectTask(string projectTaskId, [FromBody] CreateOrUpdateProjectTaskDto dto)
  {
    try
    {
      var projectTask = await _projectTaskService.UpdateProjectTask(projectTaskId, dto.Title, dto.Body, dto.Status, dto.ProjectId);
      if(projectTask == null)
        return NotFound("No Project Task Found");
      return Ok(projectTask);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpDelete("delete/{projectTaskId}/{projectId:guid}")]
  public async Task<IActionResult> DeleteProjectTask(string projectTaskId, Guid projectId)
  {
    try
    {
      var projectTask = await _projectTaskService.DeleteProjectTask(projectTaskId, projectId);
      if (projectTask == null)
        return NotFound("No Project Task Found");
      return Ok("Project Task Deleted");
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("get/{projectTaskId}/{projectId:guid}")]
  public async Task<IActionResult> GetProjectTask(string projectTaskId, Guid projectId)
  {
    try
    {
      var projectTask = await _projectTaskService.GetProjectTask(projectTaskId, projectId);
      if (projectTask == null)
        return NotFound("No Project Task Found");
      return Ok(projectTask);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpPut("updateStatus/{projectTaskId}/{projectTaskStatus}/{projectId:guid}")]
  public async Task<IActionResult> UpdateProjectTaskStatus(string projectTaskId, ProjectTaskStatus projectTaskStatus, Guid projectId)
  {
    try
    {
      var projectTask = await _projectTaskService.UpdateProjectTaskStatus(projectTaskId, projectTaskStatus, projectId);
      if (projectTask == null)
        return NotFound("No Project Task Found");
      return Ok(projectTask);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

  [HttpGet("getAllProjectTasksForProject/{projectId:guid}")]
  public async Task<IActionResult> GetAllProjectTasksForProject(Guid projectId)
  {
    try
    {
      var projectTasks = await _projectTaskService.GetAllProjectTasksForProject(projectId);
      if (projectTasks.Count == 0)
        return NotFound("No Project Tasks Found");
      return Ok(projectTasks);
    }
    catch (Exception e)
    {
      return StatusCode(500, e.Message);
    }
  }

}
