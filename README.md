A Result type for CSharp which allows for error passing without exceptions
=

This package is an attempt to allow consumers to pass custom errors upstream without having to try and catch exceptions that may change over time,
thus decreasing the maintenance cost of changes and bugfixes downstream.  
This also allows for more functionality within error with an IError interface which can be extended to provide better usability, and more flexibility.
    Ex: Having a method to return the appropriate **ObjectResult** for a failure of a rest api;

```csharp
    public interface IError 
    {
        public string Message { get; }
        
        public void Log();
        public ObjectResult GetHttpResponse();
    }

    public class NotFoundInDatabase : IError 
    {
        private string _message;
        public string Message => _message;

        public NotFoundInDatabaseError(Guid id) =>
            _message = String.Format("Item with id: {0} not found in database", id);

        public void Log() => Console.WriteLine(_message);
        
        public ObjectResult GetHttpResponse() 
        {
            ObjectResult result = new(_message);
            result.StatusCode = StatusCodes.Status404NotFound;
            return result;
        }
    }
```

Which can then be thrown by your service and consumed appropriately inside of your controller :

```csharp
    public class ItemService
    {
        private IDatabase _db;
        public ItemService(IDatabase db)
        {
            _db = db;
        }
        
        public Result<Item> GetItem(Guid id)
        {
            Item? item = db.Item.Find(id);
            return item is null
                ? Result<Item>.Fail(new NotFoundInDatabase(id)
                : Result<Item>.Ok(item);
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
            return result.Succeded
                ? Ok(result.Data)
                : result.Error.GetHttpResponse();
        }
    }

```
