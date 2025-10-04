namespace AIInGames.Planning.PDDL.Implementation
{
    internal class PDDLType : IType
    {
        public string Name { get; }
        public IType? ParentType { get; }

        public PDDLType(string name, IType? parentType = null)
        {
            Name = name;
            ParentType = parentType;
        }

        public bool IsSubtypeOf(IType type)
        {
            if (Name == type.Name) return true;
            if (ParentType == null) return false;
            return ParentType.IsSubtypeOf(type);
        }
    }
}
