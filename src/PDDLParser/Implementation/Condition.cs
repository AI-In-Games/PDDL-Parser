using System.Collections.Generic;
using System.Linq;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Condition : ICondition
    {
        public ConditionType Type { get; }
        public ILiteral? Literal { get; }
        public IReadOnlyList<ICondition> Children { get; }

        private Condition(ConditionType type, ILiteral? literal = null, IReadOnlyList<ICondition>? children = null)
        {
            Type = type;
            Literal = literal;
            Children = children ?? new List<ICondition>();
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

        public static ICondition ForAll(ICondition child)
        {
            return new Condition(ConditionType.ForAll, children: new[] { child });
        }

        public static ICondition Exists(ICondition child)
        {
            return new Condition(ConditionType.Exists, children: new[] { child });
        }
    }
}
