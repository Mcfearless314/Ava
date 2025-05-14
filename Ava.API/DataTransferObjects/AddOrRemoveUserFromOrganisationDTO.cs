namespace Ava.API.DTOs;

public class AddOrRemoveUserFromOrganisationDTO
{
  public Guid UserId { get; set; }
  public Guid OrganisationId { get; set; }
}
