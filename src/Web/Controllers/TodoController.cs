using Microsoft.AspNetCore.Mvc;
using RToora.DemoApi.Web.Entities;
using RToora.DemoApi.Web.Models;
using RToora.DemoApi.Web.Services;

namespace RToora.DemoApi.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoItemService _todoItemService;

    public TodoController(ILogger<TodoController> logger, ITodoItemService todoItemService)
    {
        _logger = logger;
        _todoItemService = todoItemService;
    }

    /// <summary>
    /// Get all Todo Items
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Returns all Todo Items</response>
    /// <response code="500">There was an error</response>
    [HttpGet(Name = "GetTodoItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Get a Todo Item by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>test</returns>
    /// <response code="200">Return specific Todo Item</response>
    /// <response code="404">Todo Item by Id was not found</response>
    /// <response code="500">There was an error</response>
    [HttpGet("{id:long}", Name = "GetTodoItemById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Creates a TodoItem.
    /// </summary>
    /// <param name="todoItem"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /Todo
    ///     {
    ///         "id": 1,
    ///         "name": "Item #1",
    ///         "isComplete": false
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">Todo Item was created.</response>
    /// <response code="404">Todo Item was not found.</response>
    /// <response code="409">Todo Item had a creation conflict.</response>
    /// <response code="400">There was a bad request.</response>
    [HttpPost(Name = "CreateTodoItem")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                    => Problem(operationResult.ErrorMessage, statusCode: StatusCodes.Status409Conflict, title: "Update Conflict"),
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

    /// <summary>
    /// Update specified Todo Item.
    /// </summary>
    /// <param name="todoItem"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Todo Item was updated</response>
    /// <response code="404">Todo Item was not found.</response>
    /// <response code="409">Todo Item had an updation conflict.</response>
    /// <response code="400">There was a bad request.</response>
    [HttpPut("{id:long}", Name = "UpdateTodoItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a specific TodoItem.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">No Content</response>
    /// <response code="404">Todo Item was not found</response>
    [HttpDelete("{id:long}", Name = "DeleteTodoItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
