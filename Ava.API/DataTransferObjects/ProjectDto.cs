namespace Ava.API.DataTransferObjects;

public class ProjectDto
{
  public Guid ProjectId { get; set; }
  public required string Title { get; set; }
  public string? SubTitle { get; set; }
}
