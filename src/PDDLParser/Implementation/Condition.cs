using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Condition : ICondition
    {
        public ConditionType Type { get; }
        public ILiteral? Literal { get; }
        public IReadOnlyList<ICondition> Children { get; }
        public IReadOnlyList<IParameter> Parameters { get; }

        private Condition(ConditionType type, ILiteral? literal = null, IReadOnlyList<ICondition>? children = null, IReadOnlyList<IParameter>? parameters = null)
        {
            Type = type;
            Literal = literal;
            Children = children ?? new List<ICondition>();
            Parameters = parameters ?? new List<IParameter>();
        }

        // Factory methods
        public static ICondition FromLiteral(ILiteral literal)
        {
            return new Condition(ConditionType.Literal, literal);
        }

        public static ICondition And(params ICondition[] children)
        {
            return new Condition(ConditionType.And, children: children.ToList());
        }

        public static ICondition Or(params ICondition[] children)
        {
            return new Condition(ConditionType.Or, children: children.ToList());
        }

        public static ICondition Not(ICondition child)
        {
            return new Condition(ConditionType.Not, children: new[] { child });
        }

        public static ICondition Imply(ICondition antecedent, ICondition consequent)
        {
            return new Condition(ConditionType.Imply, children: new[] { antecedent, consequent });
        }

        public static ICondition ForAll(IReadOnlyList<IParameter> parameters, ICondition child)
        {
            return new Condition(ConditionType.ForAll, children: new[] { child }, parameters: parameters);
        }

        public static ICondition Exists(IReadOnlyList<IParameter> parameters, ICondition child)
        {
            return new Condition(ConditionType.Exists, children: new[] { child }, parameters: parameters);
        }

        public string ToPddl()
        {
            var sb = new StringBuilder();
            AppendToPddl(sb);
            return sb.ToString();
        }

        internal void AppendToPddl(StringBuilder sb)
        {
            switch (Type)
            {
                case ConditionType.Literal:
                    if (Literal is Literal literal)
                        literal.AppendToPddl(sb);
                    else
                        sb.Append(Literal!.ToPddl());
                    break;

                case ConditionType.And:
                    sb.Append("(and");
                    foreach (var child in Children)
                    {
                        sb.Append(" ");
                        if (child is Condition childCondition)
                            childCondition.AppendToPddl(sb);
                        else
                            sb.Append(child.ToPddl());
                    }
                    sb.Append(")");
                    break;

                case ConditionType.Or:
                    sb.Append("(or");
                    foreach (var child in Children)
                    {
                        sb.Append(" ");
                        if (child is Condition childCondition)
                            childCondition.AppendToPddl(sb);
                        else
                            sb.Append(child.ToPddl());
                    }
                    sb.Append(")");
                    break;

                case ConditionType.Not:
                    sb.Append("(not ");
                    if (Children[0] is Condition childCond)
                        childCond.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(")");
                    break;

                case ConditionType.Imply:
                    sb.Append("(imply ");
                    if (Children[0] is Condition antecedent)
                        antecedent.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(" ");
                    if (Children[1] is Condition consequent)
                        consequent.AppendToPddl(sb);
                    else
                        sb.Append(Children[1].ToPddl());
                    sb.Append(")");
                    break;

                case ConditionType.ForAll:
                    sb.Append("(forall (");
                    PddlFormatHelper.AppendParameters(sb, Parameters);
                    sb.Append(") ");
                    if (Children[0] is Condition forallChild)
                        forallChild.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(")");
                    break;

                case ConditionType.Exists:
                    sb.Append("(exists (");
                    PddlFormatHelper.AppendParameters(sb, Parameters);
                    sb.Append(") ");
                    if (Children[0] is Condition existsChild)
                        existsChild.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(")");
                    break;
            }
        }
    }
}
