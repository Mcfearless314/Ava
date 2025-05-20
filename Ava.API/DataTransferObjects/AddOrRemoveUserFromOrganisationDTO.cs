namespace Ava.API.DataTransferObjects;

public class AddOrRemoveUserFromOrganisationDto
{
  public required string Username { get; set; }
  public Guid OrganisationId { get; set; }
}
