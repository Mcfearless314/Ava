using System.Security.Claims;
using System.Text.Json;
using Ava.API.Authorization.Policies;
using Ava.API.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Service.Services.Application;

namespace Ava.API.Authorization;

public class MustBeProjectManagerHandler : AuthorizationHandler<MustBeProjectManagerRequirement>
{
  private readonly ProjectService _projectService;
  private readonly IHttpContextAccessor _httpContextAccessor;

    public MustBeProjectManagerHandler(ProjectService projectService, IHttpContextAccessor httpContextAccessor)
    {
        _projectService = projectService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      MustBeProjectManagerRequirement requirement)
    {
      var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (!Guid.TryParse(userIdStr, out var userId))
      {
        context.Fail();
        return;
      }

      var request = _httpContextAccessor.HttpContext?.Request;

      Guid projectId = Guid.Empty;

      if (request?.RouteValues.TryGetValue("projectId", out var routeValue) == true &&
          Guid.TryParse(routeValue?.ToString(), out var routeProjectId))
      {
        projectId = routeProjectId;
      }
      else if (request != null)
      {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        try
        {
          using var jsonDoc = JsonDocument.Parse(body);
          if (jsonDoc.RootElement.TryGetProperty("projectId", out var idProperty) &&
              Guid.TryParse(idProperty.GetString(), out var bodyProjectId))
          {
            projectId = bodyProjectId;
          }
        }
        catch
        {
          context.Fail();
          return;
        }
      }

      if (projectId == Guid.Empty)
      {
        context.Fail();
        return;
      }

      var projectManager = await _projectService.GetProjectManager(projectId);

      if (projectManager.Id == userId)
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }
    }
}
