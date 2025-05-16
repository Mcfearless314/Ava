using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class ProjectTask
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProjectTaskStatus Status { get; set; }

    public Guid ProjectId { get; set; }

}

public enum ProjectTaskStatus
{
    ToDo = 1,
    InProgress = 2,
    Done = 3,
    Deleted = 4
}
