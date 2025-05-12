using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class ProjectTask
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public TaskStatus Status { get; set; }

    public Guid ProjectId { get; set; }
}

public enum TaskStatus
{
    ToDo = 1,
    InProgress = 2,
    Done = 3,
    Deleted = 4
}
