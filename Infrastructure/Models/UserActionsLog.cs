﻿using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class UserActionsLog
{
  [Key]
  public Guid ActionId { get; set; }

  public required UserActions ActionLog { get; set; }
  public required string Description { get; set; }
  public required DateTime ActionPerformedAt { get; set; }
  public required Guid ActionUser { get; set; }
  public Guid? TargetUser { get; set; }
  public string? ProjectTaskId { get; set; }
  public Guid? ProjectId { get; set; }
  public Guid? OrganisationId { get; set; }
}

public enum UserActions
{
  // Projects
  CreateProject = 1,
  UpdateProject = 2,
  DeleteProject = 3,
  AddUserToProject = 4,
  RemoveUserFromProject = 5,

  // Tasks
  CreateProjectTask = 6,
  UpdateProjectTask = 7,
  DeleteProjectTask = 8,
  UpdateProjectTaskStatus = 9,

  // Organisation
  AddUserToOrganisation = 10,
  RemoveUserFromOrganisation = 11,
  AssignedProjectManager = 12,
  UpdatedOrganisation = 13,
  CreateOrganisation = 14,
  DeleteOrganisation = 15,

}
