
namespace Domain.Abstractions
{

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public Error Error { get; private set; }
        public static Result Success() => new Result { IsSuccess = true, Error = Error.None };
        public static Result Failure(Error error) => new Result { IsSuccess = false, Error = error };
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public Error Error { get; private set; }

        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data, Error = Error.None };
        public static Result<T> Failure(Error error) => new Result<T> { IsSuccess = false, Error = error };
    }

}
