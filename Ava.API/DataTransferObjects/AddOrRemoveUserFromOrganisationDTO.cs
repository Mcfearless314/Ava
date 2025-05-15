namespace Ava.API.DataTransferObjects;

public class AddOrRemoveUserFromOrganisationDto
{
  public Guid UserId { get; set; }
  public Guid OrganisationId { get; set; }
}
