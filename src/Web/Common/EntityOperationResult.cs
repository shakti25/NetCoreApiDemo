namespace RToora.DemoApi.Web.Common;

public struct EntityOperationResult<TEntity> where TEntity : class
{
    public EntityOperationResult(OperationResultType operationResult, TEntity? entity = null, string? message = null)
    {
        OperationResult = operationResult;
        Entity = entity;
        ErrorMessage = message;
    }

    public TEntity? Entity { get; }
    public string? ErrorMessage { get; }
    public OperationResultType OperationResult { get; }
}

public enum OperationResultType
{
    Created,
    Modified,
    Conflict,
    InvalidInput,
    UnexpectedError,
    ConcurrencyError,
    NotFound
}