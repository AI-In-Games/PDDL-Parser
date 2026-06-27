#if ENABLE_VALIDATION
using System.Collections.Generic;

namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// The outcome of running VAL against PDDL input.
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// Whether VAL reported the input as valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// True when the Validate binary could not be located for the current platform.
        /// When set, <see cref="IsValid"/> is false and the result reflects a tooling
        /// problem rather than an invalid plan or domain.
        /// </summary>
        public bool BinaryMissing { get; private set; }

        /// <summary>
        /// The error lines extracted from VAL's output. Empty when the input is valid.
        /// </summary>
        public IReadOnlyList<string> Errors { get; private set; } = new List<string>();

        /// <summary>
        /// The full, unparsed output produced by VAL.
        /// </summary>
        public string RawOutput { get; private set; } = string.Empty;

        private ValidationResult() { }

        internal static ValidationResult Valid(string rawOutput) => new ValidationResult
        {
            IsValid = true,
            RawOutput = rawOutput
        };

        internal static ValidationResult Invalid(IReadOnlyList<string> errors, string rawOutput) => new ValidationResult
        {
            IsValid = false,
            Errors = errors,
            RawOutput = rawOutput
        };

        internal static ValidationResult NoBinary() => new ValidationResult
        {
            BinaryMissing = true,
            IsValid = false,
            Errors = new List<string> { "Validate binary not found. See Binaries/BUILD.md for build instructions." }
        };
    }
}
#endif
