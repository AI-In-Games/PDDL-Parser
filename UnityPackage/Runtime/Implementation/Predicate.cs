using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Predicate : IPredicate
    {
        public string Name { get; }
        public IReadOnlyList<IParameter> Parameters { get; }
        public int Arity => Parameters.Count;

        public Predicate(string name, IReadOnlyList<IParameter> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string ToPddl()
        {
            var sb = new StringBuilder();
            AppendToPddl(sb, 2);
            return sb.ToString();
        }

        internal void AppendToPddl(StringBuilder sb, int indent)
        {
            PddlFormatHelper.AppendIndent(sb, indent);
            sb.Append("(");
            sb.Append(Name);
            if (Parameters.Count > 0)
            {
                sb.Append(" ");
                PddlFormatHelper.AppendParameters(sb, Parameters);
            }
            sb.Append(")");
        }
    }
}
