using System;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using AIInGames.Planning.PDDL.Errors;
using AIInGames.Planning.PDDL.Generated;
using AIInGames.Planning.PDDL.Visitors;

namespace AIInGames.Planning.PDDL
{
    /// <summary>
    /// Main PDDL parser implementation
    /// </summary>
    public class PDDLParser : IPDDLParser
    {
        public IParseResult<IDomain> ParseDomain(string domainText)
        {
            if (string.IsNullOrWhiteSpace(domainText))
            {
                return ParseResult<IDomain>.Failure(
                    new ParseError("Domain text cannot be null or empty", 0, 0, ErrorSeverity.Error)
                );
            }

            try
            {
                var inputStream = new AntlrInputStream(domainText);
                var lexer = new PddlLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new PddlParser(tokenStream);

                // Add custom error listener
                var errorListener = new ParseErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                // Parse the domain
                var domainContext = parser.domain();

                // Check for parse errors
                if (errorListener.Errors.Any())
                {
                    return ParseResult<IDomain>.Failure(errorListener.Errors);
                }

                // Visit the parse tree to build the domain model
                var visitor = new DomainVisitor();
                var domain = visitor.VisitDomain(domainContext);

                return ParseResult<IDomain>.Successful(domain);
            }
            catch (Exception ex)
            {
                return ParseResult<IDomain>.Failure(
                    new ParseError($"Unexpected error: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
        }

        public IParseResult<IProblem> ParseProblem(string problemText)
        {
            if (string.IsNullOrWhiteSpace(problemText))
            {
                return ParseResult<IProblem>.Failure(
                    new ParseError("Problem text cannot be null or empty", 0, 0, ErrorSeverity.Error)
                );
            }

            try
            {
                var inputStream = new AntlrInputStream(problemText);
                var lexer = new PddlLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new PddlParser(tokenStream);

                // Add custom error listener
                var errorListener = new ParseErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                // Parse the problem
                var problemContext = parser.problem();

                // Check for parse errors
                if (errorListener.Errors.Any())
                {
                    return ParseResult<IProblem>.Failure(errorListener.Errors);
                }

                // Visit the parse tree to build the problem model
                var visitor = new ProblemVisitor();
                var problem = visitor.VisitProblem(problemContext);

                return ParseResult<IProblem>.Successful(problem);
            }
            catch (Exception ex)
            {
                return ParseResult<IProblem>.Failure(
                    new ParseError($"Unexpected error: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
        }

        public IParseResult<IDomain> ParseDomainFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return ParseResult<IDomain>.Failure(
                        new ParseError($"Domain file not found: {filePath}", 0, 0, ErrorSeverity.Error)
                    );
                }

                var domainText = File.ReadAllText(filePath);
                return ParseDomain(domainText);
            }
            catch (IOException ex)
            {
                return ParseResult<IDomain>.Failure(
                    new ParseError($"IO error reading domain file: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return ParseResult<IDomain>.Failure(
                    new ParseError($"Access denied reading domain file: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
        }

        public IParseResult<IProblem> ParseProblemFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return ParseResult<IProblem>.Failure(
                        new ParseError($"Problem file not found: {filePath}", 0, 0, ErrorSeverity.Error)
                    );
                }

                var problemText = File.ReadAllText(filePath);
                return ParseProblem(problemText);
            }
            catch (IOException ex)
            {
                return ParseResult<IProblem>.Failure(
                    new ParseError($"IO error reading problem file: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return ParseResult<IProblem>.Failure(
                    new ParseError($"Access denied reading problem file: {ex.Message}", 0, 0, ErrorSeverity.Error)
                );
            }
        }
    }
}
