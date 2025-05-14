using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IOrganizationRepository
{
  public Task<Organisation> CreateOrganisation(string name, Guid userId);

  public Task<Organisation> UpdateOrganisation(Guid organisationId, string name);

  public Task DeleteOrganisation(Guid organisationId);

  public Task AddUserToOrganisation(Guid userId, Guid organisationId);

  public Task RemoveUserFromOrganisation(Guid userId, Guid organisationId);
}
