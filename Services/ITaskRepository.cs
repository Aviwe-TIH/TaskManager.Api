using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync(bool? completed = null);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<bool> CreateTaskAsync(TaskItem taskItem);
    Task<bool> UpdateTaskAsync(TaskItem taskItem);
    Task<bool> DeleteTaskAsync(int id);
}