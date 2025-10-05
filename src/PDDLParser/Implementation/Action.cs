using System.Collections.Generic;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Action : IAction
    {
        public string Name { get; }
        public IReadOnlyList<IParameter> Parameters { get; }
        public ICondition Precondition { get; }
        public IEffect Effect { get; }

        public Action(string name, IReadOnlyList<IParameter> parameters, ICondition precondition, IEffect effect)
        {
            Name = name;
            Parameters = parameters;
            Precondition = precondition;
            Effect = effect;
        }

        public string ToPddl()
        {
            var sb = new StringBuilder();
            AppendToPddl(sb, 1);
            return sb.ToString();
        }

        internal void AppendToPddl(StringBuilder sb, int indent)
        {
            PddlFormatHelper.AppendIndent(sb, indent);
            sb.Append("(:action ");
            sb.AppendLine(Name);

            // Parameters
            PddlFormatHelper.AppendIndent(sb, indent + 1);
            sb.Append(":parameters (");
            PddlFormatHelper.AppendParameters(sb, Parameters);
            sb.AppendLine(")");

            // Precondition
            PddlFormatHelper.AppendIndent(sb, indent + 1);
            sb.Append(":precondition ");
            if (Precondition is Condition condition)
                condition.AppendToPddl(sb);
            else
                sb.Append(Precondition.ToPddl());
            sb.AppendLine();

            // Effect
            PddlFormatHelper.AppendIndent(sb, indent + 1);
            sb.Append(":effect ");
            if (Effect is Effect effect)
                effect.AppendToPddl(sb);
            else
                sb.Append(Effect.ToPddl());
            sb.AppendLine();

            PddlFormatHelper.AppendIndent(sb, indent);
            sb.Append(")");
        }
    }
}
