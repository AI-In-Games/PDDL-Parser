#if ENABLE_VALIDATION
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Default <see cref="IValidateBinaryLocator"/>. Resolves the Validate executable by, in order:
    /// an explicit path, the PDDL_VALIDATE_PATH environment variable, the resolved Unity package
    /// (editor only), then a platform-specific path under a "Binaries" folder next to the assembly.
    /// </summary>
    internal sealed class DefaultValidateBinaryLocator : IValidateBinaryLocator
    {
        /// <summary>
        /// Environment variable that, when set, overrides binary discovery with an explicit path.
        /// </summary>
        internal const string PathEnvironmentVariable = "PDDL_VALIDATE_PATH";

        private readonly string? _explicitPath;

        public DefaultValidateBinaryLocator() : this(null) { }

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

#if UNITY_EDITOR
            string? fromPackage = FindInUnityPackage(relativePath);
            if (fromPackage != null)
                return fromPackage;
#endif

            string assemblyDir = Path.GetDirectoryName(typeof(DefaultValidateBinaryLocator).Assembly.Location) ?? string.Empty;
            string candidate = Path.GetFullPath(Path.Combine(assemblyDir, "Binaries", relativePath));
            return File.Exists(candidate) ? candidate : null;
        }

#if UNITY_EDITOR
        // Resolves the binary shipped in this package using Unity's package layout, since the
        // compiled assembly does not live next to the package's Binaries folder in Unity.
        private static string? FindInUnityPackage(string platformRelativePath)
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                typeof(DefaultValidateBinaryLocator).Assembly);
            if (packageInfo == null)
                return null;

            string candidate = Path.Combine(
                packageInfo.resolvedPath, "Runtime", "Validation", "Binaries", platformRelativePath);
            return File.Exists(candidate) ? candidate : null;
        }
#endif

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
