using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using AIInGames.Planning.PDDL.Generated;
using AIInGames.Planning.PDDL.Implementation;

namespace AIInGames.Planning.PDDL.Visitors
{
    internal class DomainVisitor : PddlBaseVisitor<IDomain>
    {
        private Dictionary<string, IType> _typeMap = new Dictionary<string, IType>();
        private Dictionary<string, IPredicate> _predicateMap = new Dictionary<string, IPredicate>();

        public override IDomain VisitDomain([NotNull] PddlParser.DomainContext context)
        {
            var name = context.name().GetText();

            var requirements = context.requireDef() != null
                ? ParseRequireDef(context.requireDef())
                : new List<string>();

            var types = context.typesDef() != null
                ? ParseTypesDef(context.typesDef())
                : new List<IType>();

            var predicates = context.predicatesDef() != null
                ? ParsePredicatesDef(context.predicatesDef())
                : new List<IPredicate>();

            var actions = context.actionDef()
                .Select(ParseActionDef)
                .ToList();

            return new Domain(name, requirements, types, predicates, actions);
        }

        private List<string> ParseRequireDef(PddlParser.RequireDefContext context)
        {
            return context.requireKey()
                .Select(rk => rk.GetText())
                .ToList();
        }

        private List<IType> ParseTypesDef(PddlParser.TypesDefContext context)
        {
            // Initialize with 'object' as root type
            var objectType = new PDDLType("object", null);
            _typeMap["object"] = objectType;
            var types = new List<IType> { objectType };

            var typedList = context.typedNameList();
            if (typedList != null)
            {
                // Handle typed names (name+ '-' type)
                foreach (var singleType in typedList.singleTypeNameList())
                {
                    var typeName = singleType.type().GetText();
                    var parentType = ResolveType(typeName);

                    foreach (var nameContext in singleType.name())
                    {
                        var name = nameContext.GetText();
                        var type = new PDDLType(name, parentType);
                        _typeMap[name] = type;
                        types.Add(type);
                    }
                }

                // Handle untyped names (default to object)
                foreach (var nameContext in typedList.name())
                {
                    var name = nameContext.GetText();
                    if (!_typeMap.ContainsKey(name))
                    {
                        var type = new PDDLType(name, objectType);
                        _typeMap[name] = type;
                        types.Add(type);
                    }
                }
            }

            return types;
        }

        private List<IPredicate> ParsePredicatesDef(PddlParser.PredicatesDefContext context)
        {
            var predicates = context.atomicFormulaSkeleton()
                .Select(ParseAtomicFormulaSkeleton)
                .ToList();

            foreach (var pred in predicates)
            {
                _predicateMap[pred.Name] = pred;
            }

            return predicates;
        }

        private IPredicate ParseAtomicFormulaSkeleton(PddlParser.AtomicFormulaSkeletonContext context)
        {
            var name = context.predicate().GetText();
            var parameters = ParseTypedVariableList(context.typedVariableList());
            return new Predicate(name, parameters);
        }

        private IAction ParseActionDef(PddlParser.ActionDefContext context)
        {
            var name = context.name().GetText();
            var parameters = ParseTypedVariableList(context.typedVariableList());
            var precondition = VisitPrecondition(context.goalDesc());
            var effect = VisitEffectContext(context.effect());

            return new Action(name, parameters, precondition, effect);
        }

        private List<IParameter> ParseTypedVariableList(PddlParser.TypedVariableListContext context)
        {
            var parameters = new List<IParameter>();

            // Handle typed variables (variable+ '-' type)
            foreach (var singleType in context.singleTypeVarList())
            {
                var typeName = singleType.type().GetText();
                var type = ResolveType(typeName);

                foreach (var varContext in singleType.variable())
                {
                    var varName = varContext.GetText();
                    parameters.Add(new Parameter(varName, type));
                }
            }

            // Handle untyped variables
            foreach (var varContext in context.variable())
            {
                var varName = varContext.GetText();
                parameters.Add(new Parameter(varName, null));
            }

            return parameters;
        }

        private ICondition VisitPrecondition(PddlParser.GoalDescContext? context)
        {
            if (context == null)
            {
                // Empty precondition (always true)
                return Condition.And(); // Empty AND
            }

            return VisitGoalDesc(context);
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

        private IEffect VisitEffectContext(PddlParser.EffectContext? context)
        {
            if (context == null)
            {
                // Empty effect
                return Effect.And();
            }

            if (context is PddlParser.EffectAndContext and)
            {
                var children = and.cEffect().Select(VisitCEffect).ToArray();
                return Effect.And(children);
            }
            else if (context is PddlParser.EffectSimpleContext simple)
            {
                return VisitCEffect(simple.cEffect());
            }

            throw new System.Exception($"Unknown effect type: {context.GetType().Name}");
        }

        private IEffect VisitCEffect(PddlParser.CEffectContext context)
        {
            if (context is PddlParser.CEffectForallContext forall)
            {
                var effect = VisitEffectContext(forall.effect());
                return Effect.ForAll(effect);
            }
            else if (context is PddlParser.CEffectWhenContext when)
            {
                var condition = VisitGoalDesc(when.goalDesc());
                var effect = ParseCondEffect(when.condEffect());
                return Effect.When(condition, effect);
            }
            else if (context is PddlParser.CEffectSimpleContext simple)
            {
                return VisitPEffect(simple.pEffect());
            }

            throw new System.Exception($"Unknown conditional effect type: {context.GetType().Name}");
        }

        private IEffect ParseCondEffect(PddlParser.CondEffectContext context)
        {
            if (context.pEffect().Length > 0)
            {
                // AND of multiple effects
                var children = context.pEffect().Select(VisitPEffect).ToArray();
                return Effect.And(children);
            }
            else
            {
                // Should not happen based on grammar
                return Effect.And();
            }
        }

        private IEffect VisitPEffect(PddlParser.PEffectContext context)
        {
            if (context is PddlParser.PEffectNotContext not)
            {
                var literal = VisitAtomicFormula(not.atomicFormula(), isNegated: true);
                return Effect.FromLiteral(literal);
            }
            else if (context is PddlParser.PEffectPosContext pos)
            {
                var literal = VisitAtomicFormula(pos.atomicFormula());
                return Effect.FromLiteral(literal);
            }

            throw new System.Exception($"Unknown primitive effect type: {context.GetType().Name}");
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
                // Create a simple predicate with no typed parameters
                var parameters = arguments.Select(arg => new Parameter(arg, null) as IParameter).ToList();
                predicate = new Predicate(predicateName, parameters);
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
