namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Interface for PDDL objects that can be serialized to PDDL string format.
    /// </summary>
    public interface IPddlSerializable
    {
        /// <summary>
        /// Converts this object to its PDDL string representation.
        /// </summary>
        /// <returns>The PDDL string representation of this object.</returns>
        string ToPddl();
    }
}
