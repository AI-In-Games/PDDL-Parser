using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents an effect in PDDL - what changes when an action is executed
    /// Can be a simple literal, conjunction, conditional effect, etc.
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// The type of effect (literal, and, when, forall)
        /// </summary>
        EffectType Type { get; }

        /// <summary>
        /// For literal effects, the literal to add or delete
        /// </summary>
        ILiteral? Literal { get; }

        /// <summary>
        /// For compound effects (and), the child effects
        /// </summary>
        IReadOnlyList<IEffect> Children { get; }

        /// <summary>
        /// For conditional effects (when), the condition
        /// </summary>
        ICondition? Condition { get; }
    }

    /// <summary>
    /// Types of effects in PDDL
    /// </summary>
    public enum EffectType
    {
        Literal,
        And,
        When,      // Conditional effect
        ForAll     // Universal effect
    }
}
