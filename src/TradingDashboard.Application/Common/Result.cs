using MediatR;
using System.Net;
using TradingDashboard.Application.Abstractions.Models;

namespace TradingDashboard.Application.Common
{
    public class Result<T> : IResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; set; }
        public T? Value { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        protected Result(T value)
        {
            IsSuccess = true;
            Value = value;
            StatusCode = HttpStatusCode.OK;
            Errors = Array.Empty<Error>();

        }
        protected Result(IReadOnlyList<Error> errors, HttpStatusCode statusCode)
        {
            IsSuccess = false;
            Value = default;
            StatusCode = statusCode;
            Errors = errors;
        }

        public static Result<T> Success(T value)
        {
            return new(value);
        }

        public static Result<T> Failure(IReadOnlyList<Error> errors, HttpStatusCode httpStatusCode)
        {
            return new(errors, httpStatusCode);
        }
        public static Result<T> Failure(Error error, HttpStatusCode httpStatusCode)
        {
            return new([error], httpStatusCode);
        }

        public static Result<T> NotFound(string message)
        {

            return Failure(new Error("NotFound", message), HttpStatusCode.NotFound);
        }
        public static Result<T> NotFound(string entity, Guid id)
        {

            return Failure(new Error("NotFound", $"{entity} with id {id} was not found."), HttpStatusCode.NotFound);
        }
        public static Result<T> Conflict(string message)
       => Failure(new Error("Conflict", message), HttpStatusCode.Conflict);

        public static Result<T> Unauthorized(string message)
        {
            return Failure(new Error("Unauthorized", message), HttpStatusCode.Unauthorized);
        }

        public static Result<T> ValidationFailure(IReadOnlyList<Error> errors)
        => Failure(errors, HttpStatusCode.BadRequest);
    }

    // Non-generic version for commands that return no value
    public class Result : Result<Unit>, IResult
    {
        private Result(Unit value) : base(value) { }


        private Result(IReadOnlyList<Error> errors, HttpStatusCode statusCode)
            : base(errors, statusCode) { }

        public static Result Success()
            => new(Unit.Value);

        public new static Result NotFound(string message)
            => new([new Error("NotFound", message)], HttpStatusCode.NotFound);
        public new static Result NotFound(string entity, Guid id)
            => new([new Error("NotFound", $"{entity} with id {id} was not found.")], HttpStatusCode.NotFound);
        public new static Result Failure(Error error, HttpStatusCode httpStatusCode)
            => new([error], httpStatusCode);
        public new static Result ValidationFailure(IReadOnlyList<Error> errors)
            => new(errors, HttpStatusCode.BadRequest);
    }

    public record Error
    {
        public Error(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
