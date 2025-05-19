using System.Security.Claims;
using System.Text.Json;
using Ava.API.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Service.Services.Application;

namespace Ava.API.Authorization;

public class MustBeAdminHandler : AuthorizationHandler<MustBeAdminRequirement>
{
  private readonly OrganisationService _organisationService;
  private readonly ProjectService _projectService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public MustBeAdminHandler(OrganisationService organisationService, ProjectService projectService,
    IHttpContextAccessor httpContextAccessor)
  {
    _organisationService = organisationService;
    _projectService = projectService;
    _httpContextAccessor = httpContextAccessor;
  }

  protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAdminRequirement requirement)
  {
    var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!Guid.TryParse(userIdStr, out var userId))
    {
      context.Fail();
      return;
    }

    var httpContext = _httpContextAccessor.HttpContext;
    var request = httpContext?.Request;

    Guid organisationId = Guid.Empty;
    Guid projectId = Guid.Empty;

    if (request?.RouteValues.TryGetValue("organisationId", out var routeValue) == true &&
        Guid.TryParse(routeValue?.ToString(), out var routeOrgId))
    {
      organisationId = routeOrgId;
    }
    else if (request?.RouteValues.TryGetValue("projectId", out var projectRouteValue) == true &&
             Guid.TryParse(projectRouteValue?.ToString(), out var routeProjectId))
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

        if (organisationId == Guid.Empty &&
            jsonDoc.RootElement.TryGetProperty("organisationId", out var idProperty) &&
            Guid.TryParse(idProperty.GetString(), out var bodyOrgId))
        {
          organisationId = bodyOrgId;
        }

        if (organisationId == Guid.Empty && projectId == Guid.Empty &&
            jsonDoc.RootElement.TryGetProperty("projectId", out var projectIdProperty) &&
            Guid.TryParse(projectIdProperty.GetString(), out var bodyProjectId))
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

    if (projectId != Guid.Empty)
    {
      var project = await _projectService.GetProjectByProjectId(projectId);
      if (project != null)
      {
        organisationId = project.OrganisationId;
      }

      if (organisationId == Guid.Empty)
      {
        context.Fail();
        return;
      }
    }

    var organisationAdmin = await _organisationService.GetAdminForOrganisation(organisationId);
    if (organisationAdmin.Id == userId)
    {
      context.Succeed(requirement);
    }
    else
    {
      context.Fail();
    }
  }
}
