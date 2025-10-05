using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Domain : IDomain
    {
        public string Name { get; }
        public IReadOnlyList<string> Requirements { get; }
        public IReadOnlyList<IType> Types { get; }
        public IReadOnlyList<IPredicate> Predicates { get; }
        public IReadOnlyList<IAction> Actions { get; }

        public Domain(
            string name,
            IReadOnlyList<string> requirements,
            IReadOnlyList<IType> types,
            IReadOnlyList<IPredicate> predicates,
            IReadOnlyList<IAction> actions)
        {
            Name = name;
            Requirements = requirements;
            Types = types;
            Predicates = predicates;
            Actions = actions;
        }

        public IType? GetType(string name) => Types.FirstOrDefault(t => t.Name == name);

        public IPredicate? GetPredicate(string name) => Predicates.FirstOrDefault(p => p.Name == name);

        public IAction? GetAction(string name) => Actions.FirstOrDefault(a => a.Name == name);

        public string ToPddl()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"(define (domain {Name})");

            // Requirements
            if (Requirements.Any())
            {
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.Append("(:requirements");
                foreach (var req in Requirements)
                {
                    sb.Append($" {req}");
                }
                sb.AppendLine(")");
            }

            // Types (skip the root "object" type as it's implicit)
            var userTypes = Types.Where(t => t.Name != "object").ToList();
            if (userTypes.Any())
            {
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.Append("(:types");
                foreach (var type in userTypes)
                {
                    sb.Append(" ");
                    sb.Append(type.Name);
                    if (type.ParentType != null && type.ParentType.Name != "object")
                    {
                        sb.Append(" - ");
                        sb.Append(type.ParentType.Name);
                    }
                }
                sb.AppendLine(")");
            }

            // Predicates
            if (Predicates.Any())
            {
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.AppendLine("(:predicates");
                foreach (var predicate in Predicates)
                {
                    if (predicate is Predicate pred)
                        pred.AppendToPddl(sb, 2);
                    else
                    {
                        PddlFormatHelper.AppendIndent(sb, 2);
                        sb.Append(predicate.ToPddl());
                    }
                    sb.AppendLine();
                }
                PddlFormatHelper.AppendIndent(sb, 1);
                sb.AppendLine(")");
            }

            // Actions
            foreach (var action in Actions)
            {
                if (action is Action act)
                    act.AppendToPddl(sb, 1);
                else
                {
                    PddlFormatHelper.AppendIndent(sb, 1);
                    sb.Append(action.ToPddl());
                }
                sb.AppendLine();
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
