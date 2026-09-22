using TaskManger.Api.Models;

namespace TaskManager.Api.Services;

public class InMemoryTaskRepository : ITaskRepository
{

    // Seeded In memory storage
    private readonly List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = 1,
                Title = "Set up CI/CD pipeline",
                Description = "Configure GitHub Actions workflow for automatic builds and tests.",
                isCompleted = true,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                Priority = TaskPriority.HIGH
            },
            new TaskItem
            {
                Id = 2,
                Title = "Implement error handling middleware",
                Description = "Add global exception handling using IExceptionHandler and ProblemDetails.",
                isCompleted = true,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                Priority = TaskPriority.HIGH
            },
            new TaskItem
            {Id = 3,
                Title = "Write Postman integration tests",
                Description = "Add status code, response schema, and variable chaining assertions.",
                isCompleted = false,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Priority = TaskPriority.MEDIUM
            },
            new TaskItem
            {
                Id = 4,
                Title = "Configure rate limiting",
                Description = "Apply fixed-window rate limiter to public API endpoints.",
                isCompleted = false,
                CreatedAt = DateTime.UtcNow,
                Priority = TaskPriority.LOW
            },new TaskItem
            {
                Id = 5,
                Title = "Document API with OpenAPI/Swagger",
                Description = "Add response types, descriptions, and example payloads.",
                isCompleted = false,
                CreatedAt = DateTime.UtcNow,
                Priority = TaskPriority.MEDIUM
            }
        };
    private readonly object _lock = new(); // Object to handle race conditions

 
    public TaskItem Create(TaskItem taskItem)
    {
        lock(_lock){
            taskItem.Id = _tasks.Count == 0 ? 1: _tasks.Max(task => task.Id) + 1;
            _tasks.Add(taskItem);
        }
        return taskItem;
    }

    public bool Delete(TaskItem taskItem)
    {
        if(taskItem is null) return false;
        lock (_lock)
        {
             var taskToBeDeleted = _tasks.Find(task => task.Id == taskItem.Id);

             if(taskToBeDeleted is null) return false;
             return _tasks.Remove(taskToBeDeleted);
        }

    }

    public IEnumerable<TaskItem> GetAll()
    {
        lock (_lock)
        {
            return _tasks.ToList();
        }
    }

    public TaskItem? GetById(int id)
    {
        lock (_lock)
        {
            return _tasks.FirstOrDefault(task => task.Id == id);
        }
    }

    public bool Update(TaskItem taskItem)
    {
        lock (_lock)
        {
            var taskToBeUpdated = _tasks.FirstOrDefault(task => task.Id == taskItem.Id);
            if(taskToBeUpdated is null) return false;
            
            taskToBeUpdated.Id = taskItem.Id;
            taskToBeUpdated.Title = taskItem.Title;
            taskToBeUpdated.Description = taskItem.Description;            
            taskToBeUpdated.Priority = taskItem.Priority;
            taskToBeUpdated.isCompleted = taskItem.isCompleted;
            taskToBeUpdated.DueDate = taskItem.DueDate;
            taskToBeUpdated.CreatedAt = taskItem.CreatedAt;    
            return true;
        }
    }
}