namespace Ava.API.DataTransferObjects;

public class CreateOrganisationDto
{
  public required string Name { get; set; }
  public Guid UserId { get; set; }
}
