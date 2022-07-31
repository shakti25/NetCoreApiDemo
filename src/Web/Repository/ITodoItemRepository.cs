using RToora.DemoApi.Web.Common;
using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Repository;

public interface ITodoItemRepository
{
    Task<IReadOnlyList<TodoItemDTO>?> GetTodoItemsAsync(CancellationToken cancellationToken);
    Task<TodoItem?> GetTodoItemAsync(long id, CancellationToken cancellationToken);
    Task<EntityOperationResult<TodoItemDTO>> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);
    Task<EntityOperationResult<TodoItemDTO>> UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);
    Task<long?> DeleteTodoItemAsync(long id, CancellationToken cancellationToken);
    Task<bool> TodoItemExistsAsync(long id);
}
