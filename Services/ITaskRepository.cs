using TaskManger.Api.Models;

namespace TaskManager.Api.Services;

public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);

    TaskItem Create(TaskItem taskItem);

    bool Update(TaskItem taskItem);

    bool Delete(TaskItem taskItem);
}