namespace Ava.API.DTOs;

public class CreateProjectDto
{
  public string Title { get; set; }
  public string SubTitle { get; set; }
  public Guid OrganisationId { get; set; }
  public Guid ProjectManagerId { get; set; }
}
