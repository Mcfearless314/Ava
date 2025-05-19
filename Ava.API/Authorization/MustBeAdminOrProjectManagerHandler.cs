using Ava.API.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Ava.API.Authorization;

public class MustBeAdminOrProjectManagerHandler : AuthorizationHandler<MustBeAdminOrProjectManagerRequirement>
{
  private readonly MustBeAdminHandler _adminHandler;
  private readonly MustBeProjectManagerHandler _projectManagerHandler;

  public MustBeAdminOrProjectManagerHandler(
    MustBeAdminHandler adminHandler,
    MustBeProjectManagerHandler projectManagerHandler)
  {
    _adminHandler = adminHandler;
    _projectManagerHandler = projectManagerHandler;
  }

  protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAdminOrProjectManagerRequirement requirement)
  {
    var adminContext = new AuthorizationHandlerContext(
      new[] { new MustBeAdminRequirement() },
      context.User,
      context.Resource);

    var projectManagerContext = new AuthorizationHandlerContext(
      new[] { new MustBeProjectManagerRequirement() },
      context.User,
      context.Resource);

    await _adminHandler.HandleAsync(adminContext);
    await _projectManagerHandler.HandleAsync(projectManagerContext);

    if (adminContext.HasSucceeded || projectManagerContext.HasSucceeded)
    {
      context.Succeed(requirement);
    }
    else
    {
      context.Fail();
    }
  }
}
