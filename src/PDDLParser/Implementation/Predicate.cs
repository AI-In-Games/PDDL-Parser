using System.Collections.Generic;
using System.Linq;

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
    }
}
