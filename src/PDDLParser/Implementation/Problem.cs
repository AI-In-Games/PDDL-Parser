using System.Collections.Generic;
using System.Linq;

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

        public IObject? GetObject(string name)
        {
            return Objects.FirstOrDefault(o => o.Name == name);
        }
    }
}
