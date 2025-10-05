using System.Collections.Generic;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Literal : ILiteral
    {
        public IPredicate Predicate { get; }
        public IReadOnlyList<string> Arguments { get; }
        public bool IsNegated { get; }

        public Literal(IPredicate predicate, IReadOnlyList<string> arguments, bool isNegated = false)
        {
            Predicate = predicate;
            Arguments = arguments;
            IsNegated = isNegated;
        }

        public string ToPddl()
        {
            var sb = new StringBuilder();
            AppendToPddl(sb);
            return sb.ToString();
        }

        internal void AppendToPddl(StringBuilder sb)
        {
            if (IsNegated)
                sb.Append("(not ");

            sb.Append("(");
            sb.Append(Predicate.Name);

            foreach (var arg in Arguments)
            {
                sb.Append(" ");
                sb.Append(arg);
            }

            sb.Append(")");

            if (IsNegated)
                sb.Append(")");
        }
    }
}
