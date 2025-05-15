namespace Ava.API.DataTransferObjects;

public class CreateProjectDto
{
  public required string Title { get; set; }
  public string? SubTitle { get; set; }
  public Guid OrganisationId { get; set; }
  public Guid ProjectManagerId { get; set; }
}
