using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public Error Error { get; private set; }
        public static Result Success() => new Result { IsSuccess = true };
        public static Result Failure(Error erroer) => new Result { IsSuccess = false, Error = erroer };
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public Error Error { get; private set; }

        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
        public static Result<T> Failure(Error erroer) => new Result<T> { IsSuccess = false, Error = erroer };
    }

}
