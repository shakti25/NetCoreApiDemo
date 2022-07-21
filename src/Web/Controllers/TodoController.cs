using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly TodoContext _context;

    public TodoController(ILogger<TodoController> logger, TodoContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetTodoItem")]
    public async Task<IActionResult> Get()
    {
        // To prevent from overposting attacks we are using ItemToDTO method.
        var todoItems = await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();

        return Ok(todoItems);
    }

    [HttpGet("{id}", Name = "GetTodoItemById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Get(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        // To prevent from overposting attacks we are using ItemToDTO method.
        return Ok(ItemToDTO(todoItem));
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

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
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
        _context.TodoItems.Add(todoItem);

        await _context.SaveChangesAsync();

        // To prevent from overposting attacks we are using ItemToDTO method.
        return CreatedAtAction(nameof(Post), new { id = todoItem.Id }, ItemToDTO(todoItem));
    }

    [HttpDelete("{id}", Name = "DeleteTodoItem")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO
    {
        Id = todoItem.Id,
        Name = todoItem.Name,
        IsComplete = todoItem.IsComplete
    };
}
