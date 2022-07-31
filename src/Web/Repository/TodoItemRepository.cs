using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.Common;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Entities;
using RToora.DemoApi.Web.Helpers;
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

    public async Task<IReadOnlyList<TodoItemDTO>?> GetTodoItemsAsync(CancellationToken cancellationToken = default)
    {
        // To prevent from overposting attacks we are using ItemToDTO method.
        // Review AsNoTracking() and IReadOnlyList.
        return await _context.TodoItems.
            Select(ti => TodoItemHelper.ItemToDTO(ti)).
            AsNoTracking().
            ToListAsync();
    }

    public async Task<TodoItem?> GetTodoItemAsync(long id, CancellationToken cancellationToken = default)
    {
        var todoItem = await _context.TodoItems.SingleOrDefaultAsync(ti => ti.Id == id, cancellationToken);

        // To prevent from overposting attacks we are using ItemToDTO method.
        return todoItem != null ? todoItem : null;
    }

    public async Task<EntityOperationResult<TodoItemDTO>> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        if(todoItem is null)
        {
            return new(OperationResultType.NotFound, message: "");
        }

        try
        {
            _context.TodoItems.Add(todoItem);

            await _context.SaveChangesAsync();

            // To prevent from overposting attacks we are using ItemToDTO method.
            return new(OperationResultType.Created, TodoItemHelper.ItemToDTO(todoItem), "Todo Item was created successfully");
        }
        catch (Exception ex)
        {
            return new(OperationResultType.UnexpectedError, message: ex.ToString());
        }
    }

    public async Task<EntityOperationResult<TodoItemDTO>> UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if(todoItem is null)
            {
                return new(OperationResultType.NotFound, message: $"TodoItem with id {todoItem.Id} does not exist");
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new(OperationResultType.Modified, TodoItemHelper.ItemToDTO(todoItem), "TodoItem was updated successfully");
        }
        catch (Exception ex)
        {
            return new(OperationResultType.UnexpectedError, message: ex.ToString());
        }
    }

    public async Task<long?> DeleteTodoItemAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem is not null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> TodoItemExistsAsync(long id)
    {
        return await _context.TodoItems.AnyAsync(e => e.Id == id);
    }
}
