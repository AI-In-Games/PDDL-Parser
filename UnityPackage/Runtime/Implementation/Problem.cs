using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Problem : IProblem
    {
        public string Name { get; }
        public string DomainName { get; }
        public IReadOnlyList<IObject> Objects { get; }
        public IReadOnlyList<ILiteral> InitialState { get; }
        public ICondition Goal { get; }

        public Problem(
            string name,
            string domainName,
            IReadOnlyList<IObject> objects,
            IReadOnlyList<ILiteral> initialState,
            ICondition goal)
        {
            Name = name;
            DomainName = domainName;
            Objects = objects;
            InitialState = initialState;
            Goal = goal;
        }

        public IObject? GetObject(string name) => Objects.FirstOrDefault(o => o.Name == name);

        public string ToPddl()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"(define (problem {Name})");

            PddlFormatHelper.AppendIndent(sb, 1);
            sb.AppendLine($"(:domain {DomainName})");

            // Objects
            if (Objects.Any())
            {
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.Append("(:objects");
                foreach (var obj in Objects)
                {
                    sb.Append($" {obj.Name}");
                    if (obj.Type != null)
                    {
                        sb.Append($" - {obj.Type.Name}");
                    }
                }
                sb.AppendLine(")");
            }

            // Initial state
            if (InitialState.Any())
            {
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.AppendLine("(:init");
                foreach (var literal in InitialState)
                {
                    PddlFormatHelper.AppendIndent(sb, 2);
                    if (literal is Literal lit)
                        lit.AppendToPddl(sb);
                    else
                        sb.Append(literal.ToPddl());
                    sb.AppendLine();
                }
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.AppendLine(")");
            }

            // Goal
            PddlFormatHelper.AppendIndent(sb, 1);
            sb.Append("(:goal ");
            if (Goal is Condition condition)
                condition.AppendToPddl(sb);
            else
                sb.Append(Goal.ToPddl());
            sb.AppendLine(")");

            sb.Append(")");
            return sb.ToString();
        }
    }
}
