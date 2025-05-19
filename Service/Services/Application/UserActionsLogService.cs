using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Services.Application;

public class UserActionsLogService
{
  private readonly IUserActionsLogRepository _userActionsLogRepository;

  public UserActionsLogService(IUserActionsLogRepository userActionsLogRepository)
  {
    _userActionsLogRepository = userActionsLogRepository;
  }

  public async Task CreateLog(Guid projectId, Guid? organisationId, string projectTaskId, UserActions action, Guid actionUserId)
  {
    var actionLog = new UserActionsLog
    {
      ActionId = Guid.NewGuid(),
      ActionLog = action,
      ProjectId = projectId,
      OrganisationId = organisationId,
      ProjectTaskId = projectTaskId,
      ActionPerformedAt = DateTime.Now,
      Description = GetDescription(action),
      ActionUser = actionUserId
    };

    await _userActionsLogRepository.SaveLogAsync(actionLog);
  }
  private string GetDescription(UserActions action)
  {
    return action switch
    {
      UserActions.CreateProject => "Created project",
      UserActions.UpdateProject => "Updated project",
      UserActions.DeleteProject => "Deleted project",

      UserActions.AddUserToProject => "Added user to project",
      UserActions.RemoveUserFromProject => "Removed user from project",

      UserActions.CreateProjectTask => "Created task",
      UserActions.UpdateProjectTask => "Updated task",
      UserActions.DeleteProjectTask => "Deleted task",
      UserActions.UpdateProjectTaskStatus => "Updated task status",

      UserActions.AddUserToOrganisation => "Added user to organisation",
      UserActions.RemoveUserFromOrganisation => "Removed user from organisation",

      UserActions.AssignedProjectManager => "Assigned project manager",

      UserActions.UpdatedOrganisation => "Updated organisation",
      UserActions.CreateOrganisation => "Created organisation",
      UserActions.DeleteOrganisation => "Deleted organisation",

      _ => ""
    };
  }
}
