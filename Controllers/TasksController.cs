using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll([FromQuery] bool? completed)
    {
        var tasks = await repository.GetAllAsync(completed);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await repository.GetByIdAsync(id);
        if (task is null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TaskItem task)
    {
        var created = await repository.CreateTaskAsync(task);
        if (!created) return BadRequest("Could not create task. Duplicate ID.");

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] TaskItem task)
    {
        if (id != task.Id) return BadRequest("Route ID and body ID do not match.");

        var updated = await repository.UpdateTaskAsync(task);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await repository.DeleteTaskAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}