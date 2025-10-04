namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Represents an object in a PDDL problem (e.g., 'a - block', 'room1 - room')
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// The name of the object (e.g., "a", "b", "room1")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The type of the object, or null if untyped
        /// </summary>
        IType? Type { get; }
    }
}
