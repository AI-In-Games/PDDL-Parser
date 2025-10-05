using AIInGames.Planning.PDDL;
using NUnit.Framework;
using System.Linq;

namespace AIInGames.Planning.PDDL.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        private const string SimpleDomain = @"(define (domain blocksworld)
  (:requirements :strips)
  (:predicates
    (on ?x ?y)
    (ontable ?x)
    (clear ?x)
    (holding ?x)
    (handempty)
  )
  (:action pick-up
    :parameters (?x)
    :precondition (and (clear ?x) (ontable ?x) (handempty))
    :effect (and (not (ontable ?x)) (not (clear ?x)) (not (handempty)) (holding ?x))
  )
)";

        private const string TypedDomain = @"(define (domain logistics)
  (:requirements :strips :typing)
  (:types truck package location)
  (:predicates
    (at ?obj - package ?loc - location)
    (in ?pkg - package ?truck - truck)
    (at-truck ?truck - truck ?loc - location)
  )
  (:action load
    :parameters (?pkg - package ?truck - truck ?loc - location)
    :precondition (and (at ?pkg ?loc) (at-truck ?truck ?loc))
    :effect (and (not (at ?pkg ?loc)) (in ?pkg ?truck))
  )
)";

        private const string SimpleProblem = @"(define (problem blocks-4)
  (:domain blocksworld)
  (:objects a b c d)
  (:init
    (clear c)
    (clear a)
    (clear b)
    (clear d)
    (ontable c)
    (ontable a)
    (ontable b)
    (ontable d)
    (handempty)
  )
  (:goal (and (on d c) (on c b) (on b a)))
)";

        private const string TypedProblem = @"(define (problem log-test)
  (:domain logistics)
  (:objects
    truck1 - truck
    package1 package2 - package
    depot warehouse - location
  )
  (:init
    (at package1 depot)
    (at package2 depot)
    (at-truck truck1 depot)
  )
  (:goal (and (at package1 warehouse) (at package2 warehouse)))
)";

        [Test]
        public void SerializeDomain_SimpleDomain_ProducesValidPddl()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var parseResult = parser.ParseDomain(SimpleDomain);

            Assert.That(parseResult.Success, Is.True);

            var pddlString = parseResult.Result!.ToPddl();

            Assert.That(pddlString, Does.Contain("(define (domain blocksworld)"));
            Assert.That(pddlString, Does.Contain(":requirements"));
            Assert.That(pddlString, Does.Contain(":strips"));
            Assert.That(pddlString, Does.Contain(":predicates"));
            Assert.That(pddlString, Does.Contain(":action pick-up"));
        }

        [Test]
        public void SerializeDomain_TypedDomain_IncludesTypes()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var parseResult = parser.ParseDomain(TypedDomain);

            Assert.That(parseResult.Success, Is.True);

            var pddlString = parseResult.Result!.ToPddl();

            Assert.That(pddlString, Does.Contain(":types"));
            Assert.That(pddlString, Does.Contain("truck"));
            Assert.That(pddlString, Does.Contain("package"));
            Assert.That(pddlString, Does.Contain("location"));
        }

        [Test]
        public void RoundTrip_SimpleDomain_ProducesSameStructure()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            // Parse original
            var firstParse = parser.ParseDomain(SimpleDomain);
            Assert.That(firstParse.Success, Is.True);

            // Serialize to PDDL
            var serialized = firstParse.Result!.ToPddl();

            // Write to file for debugging
            System.IO.File.WriteAllText("serialized-test.pddl", serialized);

            // Parse again
            var secondParse = parser.ParseDomain(serialized);
            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            // Verify structure is identical
            var original = firstParse.Result!;
            var roundTripped = secondParse.Result!;

            Assert.That(roundTripped.Name, Is.EqualTo(original.Name));
            Assert.That(roundTripped.Requirements.Count, Is.EqualTo(original.Requirements.Count));
            Assert.That(roundTripped.Predicates.Count, Is.EqualTo(original.Predicates.Count));
            Assert.That(roundTripped.Actions.Count, Is.EqualTo(original.Actions.Count));
        }

        [Test]
        public void RoundTrip_TypedDomain_PreservesTypes()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseDomain(TypedDomain);
            Assert.That(firstParse.Success, Is.True);

            var serialized = firstParse.Result!.ToPddl();
            var secondParse = parser.ParseDomain(serialized);
            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            var original = firstParse.Result!;
            var roundTripped = secondParse.Result!;

            Assert.That(roundTripped.Types.Count, Is.EqualTo(original.Types.Count));

            var originalTruckType = original.GetType("truck");
            var roundTrippedTruckType = roundTripped.GetType("truck");
            Assert.That(roundTrippedTruckType, Is.Not.Null);
            Assert.That(roundTrippedTruckType!.Name, Is.EqualTo(originalTruckType!.Name));
        }

        [Test]
        public void SerializeProblem_SimpleProblem_ProducesValidPddl()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var parseResult = parser.ParseProblem(SimpleProblem);

            Assert.That(parseResult.Success, Is.True);

            var pddlString = parseResult.Result!.ToPddl();

            Assert.That(pddlString, Does.Contain("(define (problem blocks-4)"));
            Assert.That(pddlString, Does.Contain("(:domain blocksworld)"));
            Assert.That(pddlString, Does.Contain(":objects"));
            Assert.That(pddlString, Does.Contain(":init"));
            Assert.That(pddlString, Does.Contain(":goal"));
        }

        [Test]
        public void SerializeProblem_TypedProblem_IncludesTypedObjects()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var parseResult = parser.ParseProblem(TypedProblem);

            Assert.That(parseResult.Success, Is.True);

            var pddlString = parseResult.Result!.ToPddl();

            Assert.That(pddlString, Does.Contain("truck1"));
            Assert.That(pddlString, Does.Contain("- truck"));
            Assert.That(pddlString, Does.Contain("package1"));
            Assert.That(pddlString, Does.Contain("- package"));
        }

        [Test]
        public void RoundTrip_SimpleProblem_ProducesSameStructure()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseProblem(SimpleProblem);
            Assert.That(firstParse.Success, Is.True);

            var serialized = firstParse.Result!.ToPddl();
            var secondParse = parser.ParseProblem(serialized);
            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            var original = firstParse.Result!;
            var roundTripped = secondParse.Result!;

            Assert.That(roundTripped.Name, Is.EqualTo(original.Name));
            Assert.That(roundTripped.DomainName, Is.EqualTo(original.DomainName));
            Assert.That(roundTripped.Objects.Count, Is.EqualTo(original.Objects.Count));
            Assert.That(roundTripped.InitialState.Count, Is.EqualTo(original.InitialState.Count));
        }

        [Test]
        public void RoundTrip_TypedProblem_PreservesTypedObjects()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseProblem(TypedProblem);
            Assert.That(firstParse.Success, Is.True);

            var serialized = firstParse.Result!.ToPddl();
            var secondParse = parser.ParseProblem(serialized);
            Assert.That(secondParse.Success, Is.True,
                $"Second parse failed. Serialized PDDL:\n{serialized}");

            var original = firstParse.Result!;
            var roundTripped = secondParse.Result!;

            var originalTruck = original.GetObject("truck1");
            var roundTrippedTruck = roundTripped.GetObject("truck1");

            Assert.That(roundTrippedTruck, Is.Not.Null);
            Assert.That(roundTrippedTruck!.Type?.Name, Is.EqualTo(originalTruck!.Type?.Name));
        }

        [Test]
        public void RoundTrip_MultipleTimes_RemainsStable()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();

            var firstParse = parser.ParseDomain(SimpleDomain);
            var firstSerialized = firstParse.Result!.ToPddl();

            var secondParse = parser.ParseDomain(firstSerialized);
            var secondSerialized = secondParse.Result!.ToPddl();

            var thirdParse = parser.ParseDomain(secondSerialized);
            var thirdSerialized = thirdParse.Result!.ToPddl();

            // After stabilizing, serializations should be identical
            Assert.That(thirdSerialized, Is.EqualTo(secondSerialized));
        }
    }
}
