namespace Ava.API.DataTransferObjects;

public class UpdateOrganisationDto
{
  public Guid OrganisationId { get; set; }
  public required string Name { get; set; }
}
