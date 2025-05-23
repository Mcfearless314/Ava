﻿using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IOrganizationRepository
{
  public Task<Organisation> CreateOrganisation(string name, Guid userId);

  public Task<Organisation> UpdateOrganisation(Guid organisationId, string name);

  public Task DeleteOrganisation(Guid organisationId);

  public Task<(string username, Guid userId)> AddUserToOrganisation(Guid userId, Guid organisationId);

  public Task<string[]> RemoveUserFromOrganisation(Guid userId, Guid organisationId);

  Task<User> GetAdminForOrganisation(Guid organisationId);

  Task<ICollection<Project>> GetAllProjects(Guid organisationId);
  Task<Guid> GetOrganisationByProject(Guid projectId);
}
