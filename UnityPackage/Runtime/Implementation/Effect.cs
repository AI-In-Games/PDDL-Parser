using System.Collections.Generic;
using System.Linq;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Effect : IEffect
    {
        public EffectType Type { get; }
        public ILiteral? Literal { get; }
        public IReadOnlyList<IEffect> Children { get; }
        public ICondition? Condition { get; }

        private Effect(EffectType type, ILiteral? literal = null, IReadOnlyList<IEffect>? children = null, ICondition? condition = null)
        {
            Type = type;
            Literal = literal;
            Children = children ?? new List<IEffect>();
            Condition = condition;
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

        public static IEffect ForAll(IEffect effect)
        {
            return new Effect(EffectType.ForAll, children: new[] { effect });
        }
    }
}
