using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents the result of a parsing operation
    /// </summary>
    /// <typeparam name="T">The type of the parsed object (IDomain or IProblem)</typeparam>
    public interface IParseResult<out T>
    {
        /// <summary>
        /// Whether the parsing was successful
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// The parsed object, or default(T) if parsing failed
        /// </summary>
        T? Result { get; }

        /// <summary>
        /// Parse errors, if any
        /// </summary>
        IReadOnlyList<IParseError> Errors { get; }
    }

    /// <summary>
    /// Represents a parse error
    /// </summary>
    public interface IParseError
    {
        /// <summary>
        /// The error message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The line number where the error occurred (1-based)
        /// </summary>
        int Line { get; }

        /// <summary>
        /// The column number where the error occurred (1-based)
        /// </summary>
        int Column { get; }

        /// <summary>
        /// The severity of the error
        /// </summary>
        ErrorSeverity Severity { get; }
    }

    /// <summary>
    /// Severity levels for parse errors
    /// </summary>
    public enum ErrorSeverity
    {
        Warning,
        Error
    }
}
