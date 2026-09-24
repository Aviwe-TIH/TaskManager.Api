using System.ComponentModel.DataAnnotations;
namespace TaskManager.Api.Models;

public class TaskItem
{
    [Key]
    public int Id {get; set;}
    
    [StringLength(100,MinimumLength=3)]
    public required string Title {get; set;}

    [StringLength(500)]
    public required string Description {get; set;}

    public bool isCompleted {get; set;}

    public TaskPriority Priority {get; set;} = TaskPriority.MEDIUM;

    public DateTime DueDate {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}
