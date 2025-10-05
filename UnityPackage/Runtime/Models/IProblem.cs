using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a PDDL problem - defines objects, initial state, and goal
    /// </summary>
    public interface IProblem : IPddlSerializable
    {
        /// <summary>
        /// The name of the problem (e.g., "blocks-4", "gripper-x-1")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name of the domain this problem is for
        /// </summary>
        string DomainName { get; }

        /// <summary>
        /// The objects defined in this problem
        /// </summary>
        IReadOnlyList<IObject> Objects { get; }

        /// <summary>
        /// The initial state - a set of ground literals that are true initially
        /// </summary>
        IReadOnlyList<ILiteral> InitialState { get; }

        /// <summary>
        /// The goal condition that must be satisfied
        /// </summary>
        ICondition Goal { get; }

        /// <summary>
        /// Gets an object by name, or null if not found
        /// </summary>
        IObject? GetObject(string name);
    }
}
