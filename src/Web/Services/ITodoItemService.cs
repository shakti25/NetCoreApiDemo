using RToora.DemoApi.Web.Common;
using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Services
{
    public interface ITodoItemService
    {
        Task<IReadOnlyList<TodoItemDTO>?> GetTodoItemsAsync(CancellationToken cancellationToken = default);
        Task<TodoItemDTO?> GetTodoItemByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<EntityOperationResult<TodoItemDTO>> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
        Task<EntityOperationResult<TodoItemDTO>> UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
        Task<long?> DeleteTodoItemAsync(long id, CancellationToken cancellationToken = default);
    }
}
