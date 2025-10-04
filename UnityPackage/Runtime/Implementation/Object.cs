namespace AIInGames.Planning.PDDL.Implementation
{
    internal class Object : IObject
    {
        public string Name { get; }
        public IType? Type { get; }

        public Object(string name, IType? type = null)
        {
            Name = name;
            Type = type;
        }
    }
}
