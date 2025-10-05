using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a condition (goal description) in PDDL.
    /// Can be a literal, conjunction (and), disjunction (or), negation (not), etc.
    /// </summary>
    public interface ICondition : IPddlSerializable
    {
        /// <summary>
        /// The type of condition (literal, and, or, not, imply, forall, exists, etc.)
        /// </summary>
        ConditionType Type { get; }

        /// <summary>
        /// For literal conditions, the literal
        /// </summary>
        ILiteral? Literal { get; }

        /// <summary>
        /// For compound conditions (and, or), the child conditions
        /// </summary>
        IReadOnlyList<ICondition> Children { get; }

        /// <summary>
        /// For quantified conditions (forall, exists), the bound variables.
        /// Empty for non-quantified conditions.
        /// </summary>
        IReadOnlyList<IParameter> Parameters { get; }
    }

    /// <summary>
    /// Types of conditions in PDDL
    /// </summary>
    public enum ConditionType
    {
        Literal,
        And,
        Or,
        Not,
        Imply,
        ForAll,
        Exists
    }
}
