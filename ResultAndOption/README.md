A Result type for CSharp which allows for error passing without exceptions
=

This package is an attempt to allow consumers to pass custom errors upstream without having to try and catch exceptions
that may change over time,
thus decreasing the maintenance cost of changes and bugfixes downstream.  
This also allows for more functionality within error with an IError interface which can be extended to provide better
usability, and more flexibility.
Ex: Having a method to return the appropriate **ObjectResult** for a failure of a rest api;

```csharp
    public interface IError 
    {
        public string Message { get; }
        public ErrorType Type { get; }
        
        public void Log(IErrorLogger logger);
    }

    public class NotFoundInDatabase : IError 
    {
        private string _message;
        public string Message => _message;

        public NotFoundInDatabaseError(Guid id) =>
            _message = String.Format("Item with id: {0} not found in database", id);

        public void Log(IErrorLogger logger) => logger.LogError(Message);
    }
```

You can then either create an abstract class/interface to wrap all Errors into a type that is more favourable for your
application. Ex:

```csharp

public interface IHttpParsableError : IError
{
    public ObjectResult GetHttpResponse();
}
```

So NotFoundInDatabase would become:

```csharp

public class NotFoundInDatabase : IHttpParsableError
{
        private string _message;
        public string Message => _message;

        public NotFoundInDatabaseError(Guid id) =>
            _message = String.Format("Item with id: {0} not found in database", id);

        public void Log(IErrorLogger logger) => logger.LogError(Message);

        public ObjectResult GetHttpResponse() 
        {
            ObjectResult result = new(_message);
            result.StatusCode = StatusCodes.Status404NotFound;
            return result;
        }
}

```

Which can then be returned by your service and consumed appropriately inside of your controller :

```csharp
    public class ItemService
    {
        private IDatabase _db;
        public ItemService(IDatabase db) => _db = db;
        
        public Result<Item> GetItem(Guid id)
        {
            Item? item = db.Item.Find(id);
            return Result<Item>.Unknown(item, new NotFoundInDatabase(id));
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        [ApiRoute("get-item")]
        public ObjectResult GetItem(Guid id, [Service]ItemService itemService)
        {
            Result<Item> result = itemService.GetItem(id);
            if (result.Succeeded)
                return Ok(result.Data);

            IHttpParsableError? parsableError = result.Error as IHttpParsableError;
            return parsableError is null
                ? Bad(result.Error.Message)
                : parsableError.GetHttpResponse();
        }
    }

```

The extension methods can be used to parse null values to results in a simpler style and to use Piping with single
variables such as in Fsharp:

```csharp

    public class ItemService
    {
        private IDatabase _db;
        public ItemService(IDatabase db) => _db = db;
        
        public Result<Item> GetById(Guid id) => _db.Item
            .Find(id)
            .ToResult(new NotFoundInDatabase(id));
    }
```
