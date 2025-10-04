namespace AIInGames.Planning.PDDL.Errors
{
    internal class ParseError : IParseError
    {
        public string Message { get; }
        public int Line { get; }
        public int Column { get; }
        public ErrorSeverity Severity { get; }

        public ParseError(string message, int line, int column, ErrorSeverity severity = ErrorSeverity.Error)
        {
            Message = message;
            Line = line;
            Column = column;
            Severity = severity;
        }
    }
}
