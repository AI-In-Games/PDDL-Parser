using System.Collections.Generic;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    /// <summary>
    /// Helper class for common PDDL formatting operations used by implementation classes.
    /// </summary>
    internal static class PddlFormatHelper
    {
        /// <summary>
        /// The number of spaces per indentation level.
        /// Change this to adjust indentation throughout PDDL serialization.
        /// </summary>
        public const int IndentSize = 2;

        /// <summary>
        /// The indentation string repeated for each level.
        /// Cached for performance - calculated from <see cref="IndentSize"/>.
        /// </summary>
        private static readonly string IndentString = new string(' ', IndentSize);

        /// <summary>
        /// Appends a list of parameters to the StringBuilder in PDDL format.
        /// Example: "?x - block ?y - block"
        /// </summary>
        public static void AppendParameters(StringBuilder sb, IReadOnlyList<IParameter> parameters)
        {
            if (parameters.Count == 0)
                return;

            // Group parameters by type for cleaner output
            // Use empty string as a marker for null types
            var typeGroups = new Dictionary<string, List<string>>();
            foreach (var param in parameters)
            {
                var typeName = param.Type?.Name ?? "";
                if (!typeGroups.ContainsKey(typeName))
                    typeGroups[typeName] = new List<string>();
                typeGroups[typeName].Add(param.Name);
            }

            bool first = true;
            foreach (var group in typeGroups)
            {
                if (!first) sb.Append(" ");
                first = false;

                for (int i = 0; i < group.Value.Count; i++)
                {
                    if (i > 0) sb.Append(" ");
                    sb.Append(group.Value[i]);
                }

                if (group.Key != "")
                {
                    sb.Append(" - ");
                    sb.Append(group.Key);
                }
            }
        }

        /// <summary>
        /// Appends indentation to the StringBuilder.
        /// The indentation string is repeated <paramref name="level"/> times.
        /// </summary>
        /// <param name="sb">The StringBuilder to append to</param>
        /// <param name="level">The indentation level (0 = no indent, 1 = one indent, etc.)</param>
        public static void AppendIndent(StringBuilder sb, int level)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append(IndentString);
            }
        }
    }
}
