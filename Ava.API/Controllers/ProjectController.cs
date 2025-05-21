using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Application;

namespace Ava.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
  private readonly ProjectService _projectService;

  public ProjectController(ProjectService projectService)
  {
    _projectService = projectService;
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpPost("create")]
  public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
  {
    var project =
      await _projectService.CreateProject(dto.Title, dto.SubTitle, dto.OrganisationId, dto.ProjectManagerId);
    return Ok(new ProjectDto
    {
      ProjectId = project.Id,
      Title = project.Title,
      SubTitle = project.Subtitle
    });
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpPut("update/{projectId}")]
  public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] CreateProjectDto dto)
  {
    var project = await _projectService.UpdateProject(projectId, dto.Title, dto.SubTitle, Guid.NewGuid());
    return Ok(new ProjectDto
    {
      ProjectId = project.Id,
      Title = project.Title,
      SubTitle = project.Subtitle
    });
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpDelete("delete/{projectId}")]
  public async Task<IActionResult> DeleteProject(Guid projectId)
  {
    await _projectService.DeleteProject(projectId, Guid.NewGuid());
    return Ok(new { Message = "Project deleted successfully!" });
  }

  [Authorize(Policy = "MustBeAdminOrProjectUser")]
  [HttpGet("tasks/{projectId}")]
  [Obsolete("This method is deprecated. Use GetAllProjectTasksForProject instead.")]
  public async Task<IActionResult> GetProjectTasks(Guid projectId)
  {
    var tasks = await _projectService.GetProjectTasks(projectId);
    return Ok(tasks);
  }

  [HttpGet("manager/{projectId}")]
  public async Task<IActionResult> GetProjectManager(Guid projectId)
  {
    var manager = await _projectService.GetProjectManager(projectId);
    var managerDto = new UserDto
    {
      Id = manager.Id,
      Username = manager.Username
    };
    return Ok(managerDto);
  }

  [Authorize(Policy = "MustBeAdmin")]
  [HttpDelete("remove-user/{userId}/{projectId}")]
  public async Task<IActionResult> RemoveUserFromProject(Guid userId, Guid projectId)
  {
    await _projectService.RemoveUserFromProject(userId, projectId, Guid.NewGuid());
    return Ok(new { Message = "User removed from project successfully!" });
  }

  [Authorize(Policy = "MustBeProjectManager")]
  [HttpPost("addUserToProject/{projectId}/{userId}")]
  public async Task<IActionResult> AddUserToProject([FromRoute] Guid userId, [FromRoute] Guid projectId)
  {
    var user = await _projectService.AddUserToProject(userId, projectId);
    var userDto = new UserDto
    {
      Id = user.Id,
      Username = user.Username
    };
    return Ok(userDto);
  }

  [HttpGet("checkAccess/{projectId}/{userId}")]
  public async Task<IActionResult> CheckAccess([FromRoute] Guid projectId, [FromRoute] Guid userId)
  {
    if (projectId == Guid.Empty || userId == Guid.Empty)
    {
      return BadRequest("Project ID and User ID cannot be empty.");
    }

    var hasAccess = await _projectService.CheckUserAccessToProject(projectId, userId);
    return Ok(hasAccess);
  }
}
