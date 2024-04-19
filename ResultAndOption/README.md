A Result type for CSharp which allows for error passing without exceptions
=
The goal of this package is to make errors a part of the domain being modeled in the code and leaving exceptions to do
the job they were meant to do which is to warn you of exceptional circumstances.
The way i like to think about it is, errors are for the user, exceptions are for the developer.
It is also a way to make writing code which can fail more streamlined and simple.

Starting with Options.
They allow you to do more and more simply than a simple null reference.

```csharp
    public class User
    {
        private string _firstName;
        private Option<string> _lastName;
        
        // Will concat the first and last name if a last name exists
        // otherwise will return the first name.
        public string FullName() => _lastName
            .Map(last => _firstName + last) // will return an Option<string>
            .Or(_firstName);
        
        public bool HasLastName() => _lastName.IsSome();
        
        public int NameLength() => _lastName
            .Map(last => last.Length + _firstName.Length) // will return Option<int>
            .Or(_firstName.Length);
        
    }
```

Then we have Results.
Results come in two kinds, The Simple result (Result) and a result encapsulating data.
A simple result is meant to be used on actions which can mutate data but are expected to fail in the normal course
of the application, such as paying an order with insufficient funds.

where before you would do something like:

```csharp
    public void PayOrder(decimal amount) 
    {
        if (amount < _orderTotal) throw new NotEnoughFundsException(amount);
        // Order payment logic
    }
```

The consumer has no way of knowing that this operation can fail, unless the code is well documented,
and the documentation is up to date.

With a simple result it can be made more clear that this operation can fail.

```csharp
    public Result PayOrder(decimal amount)
    {
        if (amount < _orderTotal) return Result.Fail(new NotEnoughFunds(amount));
        // Order payment logic
    }
```

It is advised that all mutations are done after the checks so that a failed result describes that no operations were
performed.

These results can also be mapped so that all subsequent operations are dependant on the success of the original one.

```csharp
public class OrderPaymentProcess
{
    private Order _order;
    private User _user;
    
    public OrderPaymentProcess(Order order, User user) 
    {
        _order = order;
        _user = user;
    }
    
    public Result Process(decimal amount) => _order
        .PayOrder(amount)
        .Do(UpdateUserFunds)
        .Do(SendCustomerInvoice); // If any of the operations fail it will return a failed result
        
    public Result SendCustomerInvoice() => // send invoice;
    
    public Result UpdateUserFunds() => // updates user funds
}
```

You can also use the IfFailed method on the result to run any undo actions that might be needed.

```csharp
 public class OrderPaymentProcess
{
    private Order _order;
    private User _user;
    
    public OrderPaymentProcess(Order order, User user) 
    {
        _order = order;
        _user = user;
    }
    
    public Result Process(decimal amount) => _order
        .PayOrder(amount)
        .Do(UpdateUserFunds)
        .IfFailed(() => UndoProcess(amount))
        .Do(SendCustomerInvoice); // If any of the operations fail it will return a failed result
        
    public Result SendCustomerInvoice() => // send invoice.
    
    public Result UpdateUserFunds() => //update funds.
    
    public void UndoProcess(decimal amount) => // undo logic
}   
```

Notice that the return type of the UndoProcess method is void as it cannot fail in the normal function of the
application,
but you still get a result back from the method call as it returns the Result it was called with, as to not confuse
downstream consumers of the state of the operation.

Complex results provide the same functionality but allowing you to pass data in between calls.

```csharp
    public class OrderService
    {
        private IDatabase _database;
        
        public Result<OrderItem> CreateOrderItem(Guid productId, int amountPurchased) => _database.Products
            .Unique(id) // returns an Option<Product>
            .ToResult(new NotFoundError("Product", productId)) // Maps the option to a Result<Product> using the passed IError if its empty.
            .Map(product => OrderItem.Create(product, amountPurchased))
    }
```

Simple results and complex results can map in between each other

```csharp
    public class ProductService
    {
        private IDatabase _database;
        
        public Result Delete(Guid id)) => _database.Products
            .Unique(id)
            .ToResult(new NotFoundError("Product", productId))
            .Map(product => MangleName(product))
            .Do(product => Update(product, id))
            .IfFailed(() => UndoDeletion(id));
        
        public Result<Product> MangleName(Product product) => // mangles product name;
        public Result Update(Product updated, Guid id) => // updates db state;
        public void UndoDeletion(Guid id) => // undoes deletion;
    }
```

Both result types use an IError when they fail, which being only an interface can be easily extended
to provide any functionality needed.

```csharp
    public interface IHttpMappableError : IError
   {
       ObjectResult GetHttpResponse();
   }
```

So when you get an error you can use C# type matching to perform any specific actions required

```csharp
    public class Controller
    {
        private ProductService _service;
        public ActionResult<Product> Delete(Guid id)
        {
            Result deletionResult = _service.Delete(id);
            if (deletionReuslt.Succeeded) {
                return Ok();
            }
            if (deletionResult.Error is IHttpMappableError httpError)
            {
                return httpError.GetHttpResponse();
            }
            return new InternalServerError();
        }
    }
```

There is support for mapping simple results and complex results with asynchronous functions

```csharp
    public class ProductService
    {
        private IDatabase _database;
        
        public Task<Result> Delete(Guid id)) => _database.Products
            .Unique(id) // returns a Task of Option
            .ToResultAsync(new NotFoundError("Product", productId))
            .MapAsync(product => MangleName(product))
            .DoAsync(product => Update(product, id))
            .IfFailedAsync(() => UndoDeletion(id));
        
        public Result<Product> MangleName(Product product) => // mangles product name;
        public Task<Result> Update(Product updated, Guid id) => // updates db state;
        public Task UndoDeletion(Guid id) => // undoes deletion;
    } 
```

The Latest release of this package also contains a context result, meaning it tracks all actions
taken from the first result to the current one, allowing for simple and seamless retries.

```csharp
    public class ProductService
    {
        private IDatabase _database;
        
        public IContextResult Delete(Guid id)) => UniqueResult
            .RunAndGetContext(id)
            .Map(product => MangleName(product))
            .Map(product => Update(product, id))
            .Retry(3);
        
        private Result<Product> UniqueResult(Guid id) => _database.Products
            .Unique(id)
            .ToResult(new UnknownError);
        public Result<Product> MangleName(Product product) => // mangles product name;
        public Result Update(Product updated, Guid id) => // updates db state;
        public void UndoDeletion(Guid id) => // undoes deletion;
    }
```

Basically any function can be attached to a ContextResult and will be retried along with all failed
results in the context from top to bottom.