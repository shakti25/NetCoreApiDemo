using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Repository;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ILogger<TodoItemRepository> _logger;
    private readonly TodoContext _context;

    public TodoItemRepository(TodoContext context, ILogger<TodoItemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TodoItemDTO> CreateTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);

        await _context.SaveChangesAsync();

        // To prevent from overposting attacks we are using ItemToDTO method.
        return ItemToDTO(todoItem);
    }

    public async Task<TodoItemDTO?> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return null;
        }

        _context.TodoItems.Remove(todoItem);

        await _context.SaveChangesAsync();

        return new TodoItemDTO();
    }

    public async Task<TodoItemDTO?> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        // To prevent from overposting attacks we are using ItemToDTO method.
        return todoItem != null ? ItemToDTO(todoItem) : null;
    }

    public async Task<List<TodoItemDTO>> GetTodoItems()
    {
        // To prevent from overposting attacks we are using ItemToDTO method.
        return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
    }

    public async Task UpdateTodoItem(TodoItem todoItem)
    {
        _context.Entry(todoItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public bool TodoItemExists(long id)
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
