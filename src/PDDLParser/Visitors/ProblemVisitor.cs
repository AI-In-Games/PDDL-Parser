using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using AIInGames.Planning.PDDL.Generated;
using AIInGames.Planning.PDDL.Implementation;

namespace AIInGames.Planning.PDDL.Visitors
{
    internal class ProblemVisitor : PddlBaseVisitor<IProblem>
    {
        private Dictionary<string, IType> _typeMap = new Dictionary<string, IType>();
        private Dictionary<string, IPredicate> _predicateMap = new Dictionary<string, IPredicate>();

        public override IProblem VisitProblem([NotNull] PddlParser.ProblemContext context)
        {
            var name = context.name(0).GetText();
            var domainName = context.name(1).GetText();

            // Initialize with 'object' as root type
            var objectType = new PDDLType("object", null);
            _typeMap["object"] = objectType;

            var objects = context.objectDecl() != null
                ? VisitObjectDecl(context.objectDecl())
                : new List<IObject>();

            var initialState = VisitInit(context.init());
            var goal = VisitGoal(context.goal());

            return new Problem(name, domainName, objects, initialState, goal);
        }

        private List<IObject> VisitObjectDecl(PddlParser.ObjectDeclContext context)
        {
            var objects = new List<IObject>();
            var typedList = context.typedNameList();

            if (typedList != null)
            {
                // Handle typed objects (name+ '-' type)
                foreach (var singleType in typedList.singleTypeNameList())
                {
                    var typeName = singleType.type().GetText();
                    var type = ResolveType(typeName);

                    foreach (var nameContext in singleType.name())
                    {
                        var name = nameContext.GetText();
                        objects.Add(new Object(name, type));
                    }
                }

                // Handle untyped objects
                foreach (var nameContext in typedList.name())
                {
                    var name = nameContext.GetText();
                    objects.Add(new Object(name, null));
                }
            }

            return objects;
        }

        private List<ILiteral> VisitInit(PddlParser.InitContext context)
        {
            return context.initEl()
                .Select(VisitInitEl)
                .ToList();
        }

        private ILiteral VisitInitEl(PddlParser.InitElContext context)
        {
            return VisitLiteral(context.literal());
        }

        private ILiteral VisitLiteral(PddlParser.LiteralContext context)
        {
            var atomicFormula = context.atomicFormula();
            var isNegated = context.ChildCount > 1; // '(not ...)' has more than 1 child

            var predicateName = atomicFormula.predicate().GetText();
            var arguments = atomicFormula.term().Select(t => t.GetText()).ToList();

            // Create a simple predicate (we don't have the domain here to look up typed predicates)
            IPredicate predicate;
            if (_predicateMap.TryGetValue(predicateName, out var pred))
            {
                predicate = pred;
            }
            else
            {
                var parameters = arguments.Select(arg => new Parameter(arg, null) as IParameter).ToList();
                predicate = new Predicate(predicateName, parameters);
                _predicateMap[predicateName] = predicate;
            }

            return new Literal(predicate, arguments, isNegated);
        }

        private ICondition VisitGoal(PddlParser.GoalContext context)
        {
            return VisitGoalDesc(context.goalDesc());
        }

        private ICondition VisitGoalDesc(PddlParser.GoalDescContext context)
        {
            if (context is PddlParser.GoalSimpleContext simple)
            {
                var literal = VisitAtomicFormula(simple.atomicFormula());
                return Condition.FromLiteral(literal);
            }
            else if (context is PddlParser.GoalAndContext and)
            {
                var children = and.goalDesc().Select(VisitGoalDesc).ToArray();
                return Condition.And(children);
            }
            else if (context is PddlParser.GoalOrContext or)
            {
                var children = or.goalDesc().Select(VisitGoalDesc).ToArray();
                return Condition.Or(children);
            }
            else if (context is PddlParser.GoalNotContext not)
            {
                var child = VisitGoalDesc(not.goalDesc());
                return Condition.Not(child);
            }
            else if (context is PddlParser.GoalImplyContext imply)
            {
                var antecedent = VisitGoalDesc(imply.goalDesc(0));
                var consequent = VisitGoalDesc(imply.goalDesc(1));
                return Condition.Imply(antecedent, consequent);
            }
            else if (context is PddlParser.GoalExistsContext exists)
            {
                var child = VisitGoalDesc(exists.goalDesc());
                return Condition.Exists(child);
            }
            else if (context is PddlParser.GoalForallContext forall)
            {
                var child = VisitGoalDesc(forall.goalDesc());
                return Condition.ForAll(child);
            }

            throw new System.Exception($"Unknown goal description type: {context.GetType().Name}");
        }

        private ILiteral VisitAtomicFormula(PddlParser.AtomicFormulaContext context, bool isNegated = false)
        {
            var predicateName = context.predicate().GetText();
            var arguments = context.term().Select(t => t.GetText()).ToList();

            // Try to find predicate in map, or create a simple one
            IPredicate predicate;
            if (_predicateMap.TryGetValue(predicateName, out var pred))
            {
                predicate = pred;
            }
            else
            {
                var parameters = arguments.Select(arg => new Parameter(arg, null) as IParameter).ToList();
                predicate = new Predicate(predicateName, parameters);
                _predicateMap[predicateName] = predicate;
            }

            return new Literal(predicate, arguments, isNegated);
        }

        private IType ResolveType(string typeName)
        {
            if (_typeMap.TryGetValue(typeName, out var type))
            {
                return type;
            }

            // Default to creating a new type with 'object' as parent
            var objectType = _typeMap.ContainsKey("object") ? _typeMap["object"] : null;
            var newType = new PDDLType(typeName, objectType);
            _typeMap[typeName] = newType;
            return newType;
        }
    }
}
