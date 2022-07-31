using Microsoft.AspNetCore.Mvc;
using RToora.DemoApi.Web.Models;
using RToora.DemoApi.Web.Services;

namespace RToora.DemoApi.Web.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoItemService _todoItemService;

    public TodoController(ILogger<TodoController> logger, ITodoItemService todoItemService)
    {
        _logger = logger;
        _todoItemService = todoItemService;
    }

    [HttpGet(Name = "GetTodoItem")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var todoItems = await _todoItemService.GetTodoItemsAsync();

            return Ok(todoItems ?? Array.Empty<TodoItemDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Unexpected Error occured.");
        }
    }

    [HttpGet("{id:long}", Name = "GetTodoItemById")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoItem = await _todoItemService.GetTodoItemByIdAsync(id, cancellationToken);

            return (todoItem is not null) ?
                Ok(todoItem) :
                Problem(detail: $"TodoItem with id {id} does not exist", statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Unexpected Error occured.");
        }
    }

    [HttpPost(Name = "PostTodoItem")]
    public async Task<IActionResult> Post([FromBody]TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        try
        {
            var operationResult = await _todoItemService.CreateTodoItemAsync(todoItem, cancellationToken);

            ActionResult response = operationResult switch
            {
                { OperationResult: Common.OperationResultType.Created, Entity: { } } 
                    => Created($"api/", operationResult.Entity),
                { OperationResult: Common.OperationResultType.NotFound } 
                    => Problem(operationResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound, title: "Not Found"),
                { OperationResult: Common.OperationResultType.Conflict }
                    => Problem(operationResult.ErrorMessage, statusCode: StatusCodes.Status409Conflict, title: "Creation Conflict"),
                { OperationResult: Common.OperationResultType.InvalidInput }
                    => Problem(operationResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest, title: "Invalid Request"),
                { OperationResult: Common.OperationResultType.Created, Entity: null }
                    => Problem(detail: operationResult.ErrorMessage, title: "Unable to create TodoItem"),
                _ => Problem(detail: operationResult.ErrorMessage, title: "Unable to create TodoItem")
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Unexpected Error occured.");
        }
    }

    [HttpPut("{id:long}", Name = "UpdateTodoItem")]
    public async Task<IActionResult> Put(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        try
        {
            var operationResult = await _todoItemService.UpdateTodoItemAsync(todoItem, cancellationToken);

            ActionResult response = operationResult switch
            {
                { OperationResult: Common.OperationResultType.Modified, Entity: { } }
                    => Ok(operationResult.Entity),
                { OperationResult: Common.OperationResultType.Conflict }
                    => Problem(detail: operationResult.ErrorMessage, statusCode: StatusCodes.Status409Conflict),
                { OperationResult: Common.OperationResultType.InvalidInput }
                    => Problem(detail: operationResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest),
                { OperationResult: Common.OperationResultType.NotFound }
                    => Problem(detail: operationResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound),
                { OperationResult: Common.OperationResultType.Modified, Entity: null }
                    => Problem(detail: operationResult.ErrorMessage, title: "Unable to update todoItem"),
                _ => Problem(detail: operationResult.ErrorMessage, title: "Unable to update todoItem")
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "unexpected error occured while attempting to update.");
        }
    }

    [HttpDelete("{id:long}", Name = "DeleteTodoItem")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _todoItemService.DeleteTodoItemAsync(id, cancellationToken) is not null ?
                NoContent() :
                Problem(detail: $"a todo item with id {id} does not exist", statusCode: StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message, title: "Unexpected error ocurred.");
        }
    }

    
}
