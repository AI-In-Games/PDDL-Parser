namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Parameter : IParameter
    {
        public string Name { get; }
        public IType? Type { get; }

        public Parameter(string name, IType? type = null)
        {
            Name = name;
            Type = type;
        }
    }
}
