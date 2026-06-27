#if ENABLE_VALIDATION
namespace AIInGames.Planning.PDDL.Validation
{
    /// <summary>
    /// Validates PDDL domains, problems, and plans using the external VAL tool.
    /// </summary>
    public interface IPlanValidator
    {
        /// <summary>
        /// Validates a domain against a problem (type-checking, predicate arities, goal well-formedness).
        /// </summary>
        ValidationResult ValidateDomainAndProblem(string domainPddl, string problemPddl);

        /// <summary>
        /// Validates a parsed domain against a parsed problem by serializing them to PDDL.
        /// </summary>
        ValidationResult ValidateDomainAndProblem(IDomain domain, IProblem problem);

        /// <summary>
        /// Validates a plan against a domain and problem.
        /// The plan must be in standard PDDL plan format: one grounded action per line as
        /// <c>(action-name arg1 arg2)</c>.
        /// </summary>
        ValidationResult ValidatePlan(string domainPddl, string problemPddl, string planPddl);

        /// <summary>
        /// Validates a plan against a parsed domain and problem by serializing them to PDDL.
        /// </summary>
        ValidationResult ValidatePlan(IDomain domain, IProblem problem, string planPddl);
    }
}
#endif
