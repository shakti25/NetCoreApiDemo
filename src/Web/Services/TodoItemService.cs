using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.Common;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Helpers;
using RToora.DemoApi.Web.Models;
using RToora.DemoApi.Web.Repository;

namespace RToora.DemoApi.Web.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemService(ILogger<TodoItemService> logger, ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<IReadOnlyList<TodoItemDTO>?> GetTodoItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _todoItemRepository.GetTodoItemsAsync(cancellationToken);
        }

        public async Task<TodoItemDTO?> GetTodoItemByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var todoItem = await _todoItemRepository.GetTodoItemAsync(id, cancellationToken);

            return (todoItem is not null) ? TodoItemHelper.ItemToDTO(todoItem) : null;
        }

        public async Task<EntityOperationResult<TodoItemDTO>> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
        {
            return await _todoItemRepository.CreateTodoItemAsync(todoItem, cancellationToken);
        }

        public async Task<EntityOperationResult<TodoItemDTO>> UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
        {
            return await _todoItemRepository.UpdateTodoItemAsync(todoItem, cancellationToken);
        }

        public async Task<long?> DeleteTodoItemAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _todoItemRepository.DeleteTodoItemAsync(id, cancellationToken);
        }
    }
}
