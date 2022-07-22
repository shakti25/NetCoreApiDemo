using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Repository;

public interface ITodoItemRepository
{
    Task<List<TodoItemDTO>> GetTodoItems();
    Task<TodoItemDTO?> GetTodoItem(long id);
    Task UpdateTodoItem(TodoItem todoItem);
    Task<TodoItemDTO> CreateTodoItem(TodoItem todoItem);
    Task<TodoItemDTO?> DeleteTodoItem(long id);
    bool TodoItemExists(long id);
}
