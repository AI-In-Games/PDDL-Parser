#if ENABLE_VALIDATION
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Default <see cref="IValidateBinaryLocator"/>. Resolves the Validate executable by, in order:
    /// an explicit path, the PDDL_VALIDATE_PATH environment variable, then a platform-specific
    /// path under a "Binaries" folder next to the executing assembly.
    /// </summary>
    public sealed class DefaultValidateBinaryLocator : IValidateBinaryLocator
    {
        /// <summary>
        /// Environment variable that, when set, overrides binary discovery with an explicit path.
        /// </summary>
        public const string PathEnvironmentVariable = "PDDL_VALIDATE_PATH";

        private readonly string? _explicitPath;

        public DefaultValidateBinaryLocator() : this(null) { }

        /// <param name="explicitPath">An explicit path to the Validate executable. Takes priority over all other probing.</param>
        public DefaultValidateBinaryLocator(string? explicitPath)
        {
            _explicitPath = explicitPath;
        }

        public string? FindValidateExecutable()
        {
            if (!string.IsNullOrEmpty(_explicitPath) && File.Exists(_explicitPath))
                return _explicitPath;

            string? fromEnv = Environment.GetEnvironmentVariable(PathEnvironmentVariable);
            if (!string.IsNullOrEmpty(fromEnv) && File.Exists(fromEnv))
                return fromEnv;

            string relativePath = PlatformRelativePath;
            string assemblyDir = Path.GetDirectoryName(typeof(DefaultValidateBinaryLocator).Assembly.Location) ?? string.Empty;
            string candidate = Path.GetFullPath(Path.Combine(assemblyDir, "Binaries", relativePath));
            return File.Exists(candidate) ? candidate : null;
        }

        private static string PlatformRelativePath
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return Path.Combine("win64", "Validate.exe");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return Path.Combine("osx", "Validate");
                return Path.Combine("linux64", "Validate");
            }
        }
    }
}
#endif
