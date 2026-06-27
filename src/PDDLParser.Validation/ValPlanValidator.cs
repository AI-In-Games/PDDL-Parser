#if ENABLE_VALIDATION
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Validates PDDL domains, problems, and plans using the external VAL tool.
    /// The VAL binary is located automatically from within the package.
    /// </summary>
    public static class ValPlanValidator
    {
        private static readonly IValidateBinaryLocator DefaultLocator = new DefaultValidateBinaryLocator();

        public static ValidationResult ValidateDomainAndProblem(string domainPddl, string problemPddl)
        {
            return ValidateDomainAndProblem(domainPddl, problemPddl, DefaultLocator);
        }

        public static ValidationResult ValidateDomainAndProblem(IDomain domain, IProblem problem)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            if (problem == null) throw new ArgumentNullException(nameof(problem));
            return ValidateDomainAndProblem(domain.ToPddl(), problem.ToPddl());
        }

        public static ValidationResult ValidatePlan(string domainPddl, string problemPddl, string planPddl)
        {
            return ValidatePlan(domainPddl, problemPddl, planPddl, DefaultLocator);
        }

        public static ValidationResult ValidatePlan(IDomain domain, IProblem problem, string planPddl)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            if (problem == null) throw new ArgumentNullException(nameof(problem));
            return ValidatePlan(domain.ToPddl(), problem.ToPddl(), planPddl);
        }

        // Test seam: lets tests substitute the binary locator without exposing it publicly.
        internal static ValidationResult ValidateDomainAndProblem(
            string domainPddl, string problemPddl, IValidateBinaryLocator locator)
        {
            return Run(locator, new[] { domainPddl, problemPddl },
                files => $"-v \"{files[0]}\" \"{files[1]}\"");
        }

        internal static ValidationResult ValidatePlan(
            string domainPddl, string problemPddl, string planPddl, IValidateBinaryLocator locator)
        {
            return Run(locator, new[] { domainPddl, problemPddl, planPddl },
                files => $"-v \"{files[0]}\" \"{files[1]}\" \"{files[2]}\"");
        }

        private static ValidationResult Run(
            IValidateBinaryLocator locator, string[] contents, Func<string[], string> buildArguments)
        {
            string? exePath = locator.FindValidateExecutable();
            if (exePath == null)
                return ValidationResult.NoBinary();

            var files = new string[contents.Length];
            try
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    files[i] = Path.ChangeExtension(Path.GetTempFileName(), ".pddl");
                    File.WriteAllText(files[i], contents[i]);
                }
                return RunValidate(exePath, buildArguments(files));
            }
            finally
            {
                foreach (string file in files)
                    TryDelete(file);
            }
        }

        private static ValidationResult RunValidate(string exePath, string arguments)
        {
            var startInfo = new ProcessStartInfo(exePath, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();

                string combined = stdout + stderr;
                return process.ExitCode == 0
                    ? ValidationResult.Valid(combined)
                    : ValidationResult.Invalid(ParseErrors(combined), combined);
            }
        }

        private static List<string> ParseErrors(string output)
        {
            var errors = new List<string>();
            foreach (string line in output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0) continue;
                if (IsErrorLine(trimmed))
                    errors.Add(trimmed);
            }
            return errors;
        }

        private static bool IsErrorLine(string line)
        {
            return line.IndexOf("problem", StringComparison.OrdinalIgnoreCase) >= 0
                || line.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0
                || line.IndexOf("fail", StringComparison.OrdinalIgnoreCase) >= 0
                || line.IndexOf("Warning", StringComparison.OrdinalIgnoreCase) >= 0
                || line.IndexOf("undefined", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void TryDelete(string? path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try { File.Delete(path); } catch { /* best effort cleanup */ }
        }
    }
}
#endif
