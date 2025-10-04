using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace AIInGames.Planning.PDDL.Errors
{
    internal class ParseErrorListener : BaseErrorListener
    {
        public List<IParseError> Errors { get; } = new List<IParseError>();

        public override void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            Errors.Add(new ParseError(msg, line, charPositionInLine, ErrorSeverity.Error));
        }
    }
}
