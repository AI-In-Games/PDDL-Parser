using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Effect : IEffect
    {
        public EffectType Type { get; }
        public ILiteral? Literal { get; }
        public IReadOnlyList<IEffect> Children { get; }
        public ICondition? Condition { get; }
        public IReadOnlyList<IParameter> Parameters { get; }

        private Effect(EffectType type, ILiteral? literal = null, IReadOnlyList<IEffect>? children = null, ICondition? condition = null, IReadOnlyList<IParameter>? parameters = null)
        {
            Type = type;
            Literal = literal;
            Children = children ?? new List<IEffect>();
            Condition = condition;
            Parameters = parameters ?? new List<IParameter>();
        }

        // Factory methods
        public static IEffect FromLiteral(ILiteral literal)
        {
            return new Effect(EffectType.Literal, literal);
        }

        public static IEffect And(params IEffect[] children)
        {
            return new Effect(EffectType.And, children: children.ToList());
        }

        public static IEffect When(ICondition condition, IEffect effect)
        {
            return new Effect(EffectType.When, children: new[] { effect }, condition: condition);
        }

        public static IEffect ForAll(IReadOnlyList<IParameter> parameters, IEffect effect)
        {
            return new Effect(EffectType.ForAll, children: new[] { effect }, parameters: parameters);
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
                case EffectType.Literal:
                    if (Literal is Literal literal)
                        literal.AppendToPddl(sb);
                    else
                        sb.Append(Literal!.ToPddl());
                    break;

                case EffectType.And:
                    sb.Append("(and");
                    foreach (var child in Children)
                    {
                        sb.Append(" ");
                        if (child is Effect childEffect)
                            childEffect.AppendToPddl(sb);
                        else
                            sb.Append(child.ToPddl());
                    }
                    sb.Append(")");
                    break;

                case EffectType.When:
                    sb.Append("(when ");
                    if (Condition is Condition condition)
                        condition.AppendToPddl(sb);
                    else
                        sb.Append(Condition!.ToPddl());
                    sb.Append(" ");
                    if (Children[0] is Effect whenEffect)
                        whenEffect.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(")");
                    break;

                case EffectType.ForAll:
                    sb.Append("(forall (");
                    PddlFormatHelper.AppendParameters(sb, Parameters);
                    sb.Append(") ");
                    if (Children[0] is Effect forallEffect)
                        forallEffect.AppendToPddl(sb);
                    else
                        sb.Append(Children[0].ToPddl());
                    sb.Append(")");
                    break;
            }
        }
    }
}
