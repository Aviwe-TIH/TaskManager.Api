using Microsoft.AspNetCore.Mvc; 
using TaskManager.Api.Services;
using TaskManger.Api.Models;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskRepository repository): ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll([FromQuery] bool? completed)
    {
        var tasks = repository.GetAll();
        if (completed.HasValue)
        {
           tasks = tasks.Where(task => task.isCompleted == completed.Value); 
        }
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = repository.GetById(id); 
        if (task is null) return NotFound(); 
        return Ok(task); 

    }

    [HttpPost]
    public ActionResult Create([FromBody] TaskItem task)
    {
        var createdTask = repository.Create(task);
        if(createdTask is null) return BadRequest();
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut]
    public ActionResult UpdateTask([FromBody] TaskItem task)
    {
        return repository.Update(task)? NoContent(): NotFound();
    }
    [HttpDelete("{id:int}")]
    public ActionResult DeleteTask(int id)
    {
        var task = repository.GetAll().FirstOrDefault(task => task.Id==id);
        
        if(task is null) return NotFound();
        return repository.Delete(task)? NoContent(): NotFound();
    }
}