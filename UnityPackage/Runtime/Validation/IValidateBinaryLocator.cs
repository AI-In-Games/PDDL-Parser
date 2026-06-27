#if ENABLE_VALIDATION
namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Locates the VAL "Validate" executable for the current platform.
    /// Internal package detail; consumers use <see cref="ValPlanValidator"/>.
    /// </summary>
    internal interface IValidateBinaryLocator
    {
        /// <summary>
        /// Returns the absolute path to the Validate executable, or null if it cannot be found.
        /// </summary>
        string? FindValidateExecutable();
    }
}
#endif
