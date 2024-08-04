using ResultAndOption.Results.Commands;
using ResultAndOption.Results.Getters;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Doing
{

    
        public static Result Do(this in Result result, IResultCommand resultCommand) => result.Failed
            ? result
            : resultCommand.Do();
    
        public static Task<Result> DoAsync(this in Result result, IAsyncResultCommand resultCommand, CancellationToken? token = null) => result.Failed
            ? Task.FromResult(result)
            : resultCommand.Do(token);
    
        public static Result Do(this in Result result, ICommand command)
        {
            if (result.Succeeded)
            {
                command.Do();
            }
    
            return result;
        }
    
        public static Task<Result> DoAsync(this in Result result, IAsyncCommand command)
        {
            if (result.Succeeded)
            {
                command.Do();
            }
    
            return Task.FromResult(result);
        } 
        
        /// <summary>
        ///     Maps the result using the specified function.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="action">The function to map the result.</param>
        /// <returns>
        ///     A new result produced by the function if the original result represents a success. Otherwise, the original
        ///     result is returned.
        /// </returns>
        public static Result Do(this in Result result, Func<Result> action) => result.Failed
            ? result
            : action();
    
        /// <summary>
        /// Invokes selected action if the result is a success and returns same result
        /// </summary>
        /// <param name="result"></param>
        /// <param name="action">Action to invoke</param>
        /// <returns></returns>
        public static Result Do(this in Result result, Action action)
        {
            if (result.Succeeded)
            {
                action();
            }
            return Result.Ok();
        }
     
}