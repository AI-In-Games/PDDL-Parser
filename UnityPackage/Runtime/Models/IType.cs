namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents a type in PDDL (e.g., 'block', 'location', etc.)
    /// Types form a hierarchy in typed PDDL.
    /// </summary>
    public interface IType
    {
        /// <summary>
        /// The name of the type (e.g., "block", "vehicle")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The parent type in the type hierarchy, or null if this is a root type (object)
        /// </summary>
        IType? ParentType { get; }

        /// <summary>
        /// Checks if this type is a subtype of (or equal to) the given type
        /// </summary>
        bool IsSubtypeOf(IType type);
    }
}
