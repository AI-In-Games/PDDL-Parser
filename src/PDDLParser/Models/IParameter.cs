namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a parameter in a predicate or action (e.g., '?x - block')
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// The name of the parameter (e.g., "?x", "?from")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The type of the parameter, or null if untyped
        /// </summary>
        IType? Type { get; }
    }
}
