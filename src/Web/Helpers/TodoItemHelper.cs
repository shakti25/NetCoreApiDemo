using RToora.DemoApi.Web.Models;

namespace RToora.DemoApi.Web.Helpers;

public static class TodoItemHelper
{
    public static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO
    {
        Id = todoItem.Id,
        Name = todoItem.Name,
        IsComplete = todoItem.IsComplete
    };
}
