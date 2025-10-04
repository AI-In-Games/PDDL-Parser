using System.Collections.Generic;

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
    }
}
