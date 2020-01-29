using System.Collections.Generic;
using System.Linq;

namespace TLV.Decoder.Core.Common
{
    /// <summary>
    /// Monadic Type in order to handle failures early and return meaningful results
    /// enables us to make flow decisions based on the result rather than throwing exceptions everywhere
    /// allows for validating the whole object graph and returning all errors of the object graph to the consumer
    /// makes debugging easier and more deterministic
    /// </summary>
    public class Result
    {
        public Error[] Errors { get; }
        public bool IsSuccess { get; }

        protected Result(params Error[] errors)
        {
            Errors = errors;
            IsSuccess = false;
        }

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value) : base(true)
        {
            Value = value;
        }

        protected Result(params Error[] errors) : base(errors) { }


        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(params Error[] errors) => new Result<T>(errors);
        public static Result<T> Failure(IEnumerable<Error[]> errors) => new Result<T>(errors.SelectMany(e => e).ToArray());
    }
}