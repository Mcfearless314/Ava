using Infrastructure.Models;

namespace Ava.API.DataTransferObjects;

public class CreateOrUpdateProjectTaskDto
{
  public required string Title { get; set; }
  public string? Body { get; set; }
  public required ProjectTaskStatus Status { get; set; }
  public required Guid ProjectId { get; set; }
}
