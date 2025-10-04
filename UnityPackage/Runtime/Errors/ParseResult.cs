using System.Collections.Generic;
using System.Linq;

namespace AIInGames.Planning.PDDL.Errors
{
    internal class ParseResult<T> : IParseResult<T>
    {
        public bool Success { get; }
        public T? Result { get; }
        public IReadOnlyList<IParseError> Errors { get; }

        private ParseResult(bool success, T? result, IReadOnlyList<IParseError> errors)
        {
            Success = success;
            Result = result;
            Errors = errors;
        }

        public static IParseResult<T> Successful(T result)
        {
            return new ParseResult<T>(true, result, new List<IParseError>());
        }

        public static IParseResult<T> Failure(params IParseError[] errors)
        {
            return new ParseResult<T>(false, default, errors.ToList());
        }

        public static IParseResult<T> Failure(IEnumerable<IParseError> errors)
        {
            return new ParseResult<T>(false, default, errors.ToList());
        }
    }
}
