using Ava.API.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Ava.API.Authorization;

public class MustBeAdminOrProjectUserHandler : AuthorizationHandler<MustBeAdminOrProjectUserRequirement>
{
  private readonly MustBeAdminHandler _adminHandler;
  private readonly MustBeProjectUserHandler _projectUserHandler;

  public MustBeAdminOrProjectUserHandler(
    MustBeAdminHandler adminHandler,
    MustBeProjectUserHandler projectUserHandler)
  {
    _adminHandler = adminHandler;
    _projectUserHandler = projectUserHandler;
  }

  protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAdminOrProjectUserRequirement requirement)
  {
    var adminContext = new AuthorizationHandlerContext(
      new[] { new MustBeAdminRequirement() },
      context.User,
      context.Resource);

    var projectUserContext = new AuthorizationHandlerContext(
      new[] { new MustBeProjectUserRequirement() },
      context.User,
      context.Resource);

    await _adminHandler.HandleAsync(adminContext);
    await _projectUserHandler.HandleAsync(projectUserContext);

    if (adminContext.HasSucceeded || projectUserContext.HasSucceeded)
    {
      context.Succeed(requirement);
    }
    else
    {
      context.Fail();
    }
  }
}
