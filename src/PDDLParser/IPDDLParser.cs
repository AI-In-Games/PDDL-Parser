namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Main interface for parsing PDDL domain and problem files
    /// </summary>
    public interface IPDDLParser
    {
        /// <summary>
        /// Parses a PDDL domain from a string
        /// </summary>
        /// <param name="domainText">The PDDL domain text</param>
        /// <returns>The parsed domain or a parse result with errors</returns>
        IParseResult<IDomain> ParseDomain(string domainText);

        /// <summary>
        /// Parses a PDDL problem from a string
        /// </summary>
        /// <param name="problemText">The PDDL problem text</param>
        /// <returns>The parsed problem or a parse result with errors</returns>
        IParseResult<IProblem> ParseProblem(string problemText);

        /// <summary>
        /// Parses a PDDL domain from a file
        /// </summary>
        /// <param name="filePath">Path to the domain file</param>
        /// <returns>The parsed domain or a parse result with errors</returns>
        IParseResult<IDomain> ParseDomainFile(string filePath);

        /// <summary>
        /// Parses a PDDL problem from a file
        /// </summary>
        /// <param name="filePath">Path to the problem file</param>
        /// <returns>The parsed problem or a parse result with errors</returns>
        IParseResult<IProblem> ParseProblemFile(string filePath);
    }
}
