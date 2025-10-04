using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a predicate definition in PDDL (e.g., '(on ?x ?y - block)')
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// The name of the predicate (e.g., "on", "clear", "at")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The parameters of the predicate with their types
        /// </summary>
        IReadOnlyList<IParameter> Parameters { get; }

        /// <summary>
        /// The arity (number of parameters) of the predicate
        /// </summary>
        int Arity { get; }
    }
}
