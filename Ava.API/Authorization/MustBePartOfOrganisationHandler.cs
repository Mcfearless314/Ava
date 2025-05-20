using System.Security.Claims;
using Ava.API.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Service.Services.Application;

public class MustBePartOfOrganisationHandler : AuthorizationHandler<MustBePartOfOrganisationRequirement>
{
  private readonly UserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public MustBePartOfOrganisationHandler(UserService userService,
    IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
    _userService = userService;
  }

  protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
    MustBePartOfOrganisationRequirement requirement)
  {
    var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!Guid.TryParse(userIdStr, out var userId))
    {
      context.Fail();
      return;
    }

    var httpContext = _httpContextAccessor.HttpContext;
    var request = httpContext?.Request;

    var organisationId = Guid.Empty;


    if (request?.RouteValues.TryGetValue("organisationId", out var routeValue) == true &&
        Guid.TryParse(routeValue?.ToString(), out var routeOrgId))
    {
      organisationId = routeOrgId;
    }

    if (organisationId == Guid.Empty)
    {
      context.Fail();
      return;
    }

    var organisationUsers = await _userService.GetAllUsers(organisationId);

    if (organisationUsers.Any(u => u.Id == userId))
    {
      context.Succeed(requirement);
    }
    else
    {
      context.Fail();
    }


  }
}
