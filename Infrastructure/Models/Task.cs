using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class Task
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Status { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
