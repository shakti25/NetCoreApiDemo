using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Models;
using RToora.DemoApi.Web.Repository;

namespace RToora.DemoApi.Web.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoController(ILogger<TodoController> logger, ITodoItemRepository todoItemRepository)
    {
        _logger = logger;
        _todoItemRepository = todoItemRepository;
    }

    [HttpGet(Name = "GetTodoItem")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _todoItemRepository.GetTodoItems());
    }

    [HttpGet("{id}", Name = "GetTodoItemById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Get(long id)
    {
        var todoItem = await _todoItemRepository.GetTodoItem(id);

        if(todoItem == null)
        {
            return NotFound();
        }
        
        return Ok(todoItem);
    }

    [HttpPut("{id}", Name = "UpdateTodoItem")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(long id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        try
        {
            await _todoItemRepository.UpdateTodoItem(todoItem);
        }
        catch (DbUpdateConcurrencyException) when (!_todoItemRepository.TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost(Name = "PostTodoItem")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Post(TodoItem todoItem)
    {
        var todoItemDto = await _todoItemRepository.CreateTodoItem(todoItem);

        return CreatedAtAction(nameof(Post), new { id = todoItem.Id }, todoItemDto);
    }

    [HttpDelete("{id}", Name = "DeleteTodoItem")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(long id)
    {
        var todoItem = await _todoItemRepository.DeleteTodoItem(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    
}
