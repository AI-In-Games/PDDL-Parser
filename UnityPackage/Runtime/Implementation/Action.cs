using System.Collections.Generic;

namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Action : IAction
    {
        public string Name { get; }
        public IReadOnlyList<IParameter> Parameters { get; }
        public ICondition Precondition { get; }
        public IEffect Effect { get; }

        public Action(string name, IReadOnlyList<IParameter> parameters, ICondition precondition, IEffect effect)
        {
            Name = name;
            Parameters = parameters;
            Precondition = precondition;
            Effect = effect;
        }
    }
}
