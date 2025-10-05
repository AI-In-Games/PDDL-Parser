using System.Collections.Generic;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents an action (operator) in PDDL with parameters, preconditions, and effects
    /// </summary>
    public interface IAction : IPddlSerializable
    {
        /// <summary>
        /// The name of the action (e.g., "move", "pick-up", "stack")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The parameters of the action
        /// </summary>
        IReadOnlyList<IParameter> Parameters { get; }

        /// <summary>
        /// The precondition formula (what must be true for the action to be applicable)
        /// </summary>
        ICondition Precondition { get; }

        /// <summary>
        /// The effect formula (what changes when the action is executed)
        /// </summary>
        IEffect Effect { get; }
    }
}
