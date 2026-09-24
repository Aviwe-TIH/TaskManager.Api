using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public class TaskService(AppDbContext db, ILogger<TaskService> logger) : ITaskRepository
{

    public async Task<IEnumerable<TaskItem>> GetAllAsync(bool? completed = null)
    {
        IQueryable<TaskItem> query = db.Tasks;

        if (completed.HasValue)
        {
            query = query.Where(t => t.isCompleted == completed.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await db.Tasks.FindAsync(id);
    }

    public async Task<bool> CreateTaskAsync(TaskItem taskItem)
    {
        logger.LogDebug("Creating task item with title: {Title}", taskItem.Title);

        if (taskItem.Id > 0)
        {
            var exists = await db.Tasks.AnyAsync(t => t.Id == taskItem.Id);
            if (exists)
            {
                logger.LogWarning("Task with ID {Id} already exists", taskItem.Id);
                return false;
            }
        }

        await db.Tasks.AddAsync(taskItem);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTaskAsync(TaskItem taskItem)
    {
        var existing = await db.Tasks.FindAsync(taskItem.Id);
        if (existing is null) return false;

        existing.Title = taskItem.Title;
        existing.Description = taskItem.Description;
        existing.Priority = taskItem.Priority;
        existing.isCompleted = taskItem.isCompleted;
        existing.DueDate = taskItem.DueDate;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return false;

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }
}