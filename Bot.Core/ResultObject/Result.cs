using Bot.Core.Errors;

namespace Bot.Core.ResultObject;

public abstract class Result
{
    public bool Success { get; protected set; }
    public bool Failure => !Success;

    public static implicit operator bool(Result result) => result.Success;
}

public abstract class Result<T> : Result
{
    public T Value { get; }

    protected Result(T data)
    {
        Value = data;
    }
}

public class SuccessResult : Result
{
    public SuccessResult()
    {
        Success = true;
    }    
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
    public static implicit operator SuccessResult<T>(T value) => new SuccessResult<T>(value);
}

public class ErrorResult : Result
{
    public Error Error { get; }   

    public ErrorResult(Error error)
    {       
        Success = false;
        Error = error;
    }
    
}

public class ErrorResult<T> : Result<T>
{
    public Error Error { get; }
    public ErrorResult(Error error) : base(default)
    {
        
        Success = false;
        Error = error;
    }      
}
