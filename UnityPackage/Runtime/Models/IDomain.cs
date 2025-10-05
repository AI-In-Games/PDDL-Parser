using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a PDDL domain - defines types, predicates, and actions
    /// </summary>
    public interface IDomain : IPddlSerializable
    {
        /// <summary>
        /// The name of the domain (e.g., "blocksworld", "gripper")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The PDDL requirements used by this domain (e.g., ":strips", ":typing", ":negative-preconditions")
        /// </summary>
        IReadOnlyList<string> Requirements { get; }

        /// <summary>
        /// The types defined in this domain (empty if untyped)
        /// </summary>
        IReadOnlyList<IType> Types { get; }

        /// <summary>
        /// The predicates defined in this domain
        /// </summary>
        IReadOnlyList<IPredicate> Predicates { get; }

        /// <summary>
        /// The actions (operators) defined in this domain
        /// </summary>
        IReadOnlyList<IAction> Actions { get; }

        /// <summary>
        /// Gets a type by name, or null if not found
        /// </summary>
        IType? GetType(string name);

        /// <summary>
        /// Gets a predicate by name, or null if not found
        /// </summary>
        IPredicate? GetPredicate(string name);

        /// <summary>
        /// Gets an action by name, or null if not found
        /// </summary>
        IAction? GetAction(string name);
    }
}
