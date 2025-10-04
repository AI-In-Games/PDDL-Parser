using System.Collections.Generic;
using System.Linq;

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

        public IType? GetType(string name)
        {
            return Types.FirstOrDefault(t => t.Name == name);
        }

        public IPredicate? GetPredicate(string name)
        {
            return Predicates.FirstOrDefault(p => p.Name == name);
        }

        public IAction? GetAction(string name)
        {
            return Actions.FirstOrDefault(a => a.Name == name);
        }
    }
}
