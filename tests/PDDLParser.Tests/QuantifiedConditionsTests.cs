using AIInGames.Planning.PDDL;
using NUnit.Framework;

namespace AIInGames.Planning.PDDL.Tests
{
    [TestFixture]
    public class QuantifiedConditionsTests
    {
        private const string DomainWithForAll = @"(define (domain quantified-test)
  (:requirements :strips :typing :universal-preconditions)
  (:types block location)
  (:predicates
    (at ?b - block ?l - location)
    (clear ?b - block)
    (empty ?l - location)
  )
  (:action move-all-clear
    :parameters (?from - location ?to - location)
    :precondition (and
      (forall (?b - block)
        (clear ?b))
      (empty ?to))
    :effect (and
      (not (empty ?to))
      (empty ?from))
  )
)";

        private const string ProblemWithExists = @"(define (problem quantified-problem)
  (:domain quantified-test)
  (:objects
    b1 b2 - block
    l1 l2 - location
  )
  (:init
    (at b1 l1)
    (at b2 l1)
    (clear b1)
    (empty l2)
  )
  (:goal (and
    (exists (?b - block)
      (at ?b l2))
    (empty l1)))
)";

        [Test]
        public void ParseDomain_WithForAll_CapturesParameters()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var result = parser.ParseDomain(DomainWithForAll);

            Assert.That(result.Success, Is.True);
            var domain = result.Result!;
            var action = domain.GetAction("move-all-clear");
            Assert.That(action, Is.Not.Null);

            // Check that precondition is an AND
            Assert.That(action!.Precondition.Type, Is.EqualTo(ConditionType.And));

            // First child should be the forall
            var forallCondition = action.Precondition.Children[0];
            Assert.That(forallCondition.Type, Is.EqualTo(ConditionType.ForAll));

            // Check parameters are captured
            Assert.That(forallCondition.Parameters.Count, Is.EqualTo(1));
            Assert.That(forallCondition.Parameters[0].Name, Is.EqualTo("?b"));
            Assert.That(forallCondition.Parameters[0].Type?.Name, Is.EqualTo("block"));
        }

        [Test]
        public void ParseProblem_WithExists_CapturesParameters()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var result = parser.ParseProblem(ProblemWithExists);

            Assert.That(result.Success, Is.True);
            var problem = result.Result!;

            // Check that goal is an AND
            Assert.That(problem.Goal.Type, Is.EqualTo(ConditionType.And));

            // First child should be the exists
            var existsCondition = problem.Goal.Children[0];
            Assert.That(existsCondition.Type, Is.EqualTo(ConditionType.Exists));

            // Check parameters are captured
            Assert.That(existsCondition.Parameters.Count, Is.EqualTo(1));
            Assert.That(existsCondition.Parameters[0].Name, Is.EqualTo("?b"));
            Assert.That(existsCondition.Parameters[0].Type?.Name, Is.EqualTo("block"));
        }

        [Test]
        public void RoundTrip_DomainWithForAll_PreservesQuantifiers()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseDomain(DomainWithForAll);
            Assert.That(firstParse.Success, Is.True);

            var serialized = firstParse.Result!.ToPddl();
            var secondParse = parser.ParseDomain(serialized);

            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            // Check forall is preserved
            var action = secondParse.Result!.GetAction("move-all-clear");
            var forallCondition = action!.Precondition.Children[0];
            Assert.That(forallCondition.Type, Is.EqualTo(ConditionType.ForAll));
            Assert.That(forallCondition.Parameters.Count, Is.EqualTo(1));
            Assert.That(forallCondition.Parameters[0].Name, Is.EqualTo("?b"));
        }

        [Test]
        public void RoundTrip_ProblemWithExists_PreservesQuantifiers()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseProblem(ProblemWithExists);
            Assert.That(firstParse.Success, Is.True);

            var serialized = firstParse.Result!.ToPddl();
            var secondParse = parser.ParseProblem(serialized);

            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            // Check exists is preserved
            var existsCondition = secondParse.Result!.Goal.Children[0];
            Assert.That(existsCondition.Type, Is.EqualTo(ConditionType.Exists));
            Assert.That(existsCondition.Parameters.Count, Is.EqualTo(1));
            Assert.That(existsCondition.Parameters[0].Name, Is.EqualTo("?b"));
        }

        [Test]
        public void Serialize_ForAllCondition_ProducesValidPddl()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var result = parser.ParseDomain(DomainWithForAll);
            Assert.That(result.Success, Is.True);

            var pddlString = result.Result!.ToPddl();

            // Check serialized output contains forall with parameters
            Assert.That(pddlString, Does.Contain("(forall (?b - block)"));
            Assert.That(pddlString, Does.Contain("(clear ?b)"));
        }
    }
}
