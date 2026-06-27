#if ENABLE_VALIDATION
namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Locates the VAL "Validate" executable for the current platform.
    /// Abstracted so the validator can run on plain .NET, in the Unity Editor, and in
    /// Unity player builds without depending on any one host's package APIs.
    /// </summary>
    public interface IValidateBinaryLocator
    {
        /// <summary>
        /// Returns the absolute path to the Validate executable, or null if it cannot be found.
        /// </summary>
        string? FindValidateExecutable();
    }
}
#endif
