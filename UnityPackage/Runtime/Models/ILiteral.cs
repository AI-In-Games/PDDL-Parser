using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a literal in PDDL - a predicate with specific arguments
    /// Can be positive or negative (e.g., '(on a b)' or '(not (clear a))')
    /// </summary>
    public interface ILiteral
    {
        /// <summary>
        /// The predicate this literal is based on
        /// </summary>
        IPredicate Predicate { get; }

        /// <summary>
        /// The arguments (variable names or object names) for this literal
        /// </summary>
        IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// Whether this is a negated literal
        /// </summary>
        bool IsNegated { get; }
    }
}
