using AIInGames.Planning.PDDL;
using NUnit.Framework;

namespace AIInGames.Planning.PDDL.Tests
{
    [TestFixture]
    public class DomainParserTests
    {
        private const string SimpleDomain = @"
(define (domain blocksworld)
  (:requirements :strips)
  (:predicates (on ?x ?y)
               (ontable ?x)
               (clear ?x)
               (holding ?x)
               (handempty))
  (:action pick-up
     :parameters (?x)
     :precondition (and (clear ?x) (ontable ?x) (handempty))
     :effect (and (not (ontable ?x))
                  (not (clear ?x))
                  (not (handempty))
                  (holding ?x)))
)";

        private const string TypedDomain = @"
(define (domain logistics)
  (:requirements :strips :typing)
  (:types truck package location)
  (:predicates (at ?obj - package ?loc - location)
               (in ?pkg - package ?truck - truck)
               (at-truck ?truck - truck ?loc - location))
  (:action load
     :parameters (?pkg - package ?truck - truck ?loc - location)
     :precondition (and (at ?pkg ?loc) (at-truck ?truck ?loc))
     :effect (and (not (at ?pkg ?loc))
                  (in ?pkg ?truck)))
)";

        [Test]
        public void ParseDomain_SimpleDomain_ReturnsDomain()
        {
            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var result = parser.ParseDomain(SimpleDomain);

            Assert.That(result.Success, Is.True, "Parsing should succeed");
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result!.Name, Is.EqualTo("blocksworld"));
        }

        [Test]
        public void ParseDomain_SimpleDomain_ParsesRequirements()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseDomain(SimpleDomain);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Requirements, Does.Contain(":strips"));
        }

        [Test]
        public void ParseDomain_SimpleDomain_ParsesPredicates()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseDomain(SimpleDomain);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Predicates.Count, Is.EqualTo(5));

            var onPredicate = result.Result!.GetPredicate("on");
            Assert.That(onPredicate, Is.Not.Null);
            Assert.That(onPredicate.Arity, Is.EqualTo(2));
        }

        [Test]
        public void ParseDomain_SimpleDomain_ParsesActions()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseDomain(SimpleDomain);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Actions.Count, Is.EqualTo(1));

            var pickupAction = result.Result!.GetAction("pick-up");
            Assert.That(pickupAction, Is.Not.Null);
            Assert.That(pickupAction.Parameters.Count, Is.EqualTo(1));
        }

        [Test]
        public void ParseDomain_TypedDomain_ParsesTypes()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseDomain(TypedDomain);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Types.Count, Is.EqualTo(4)); // 3 custom types + 'object' root type

            var truckType = result.Result!.GetType("truck");
            Assert.That(truckType, Is.Not.Null);
        }

        [Test]
        public void ParseDomain_TypedDomain_ParsesTypedPredicates()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseDomain(TypedDomain);


            Assert.That(result.Success, Is.True);

            var atPredicate = result.Result!.GetPredicate("at");
            Assert.That(atPredicate, Is.Not.Null);
            Assert.That(atPredicate.Arity, Is.EqualTo(2));
            Assert.That(atPredicate.Parameters[0].Type?.Name, Is.EqualTo("package"));
            Assert.That(atPredicate.Parameters[1].Type?.Name, Is.EqualTo("location"));
        }

        [Test]
        public void ParseDomain_InvalidSyntax_ReturnsErrors()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var invalidDomain = "(define (domain test) missing-closing-paren";


            var result = parser.ParseDomain(invalidDomain);


            Assert.That(result.Success, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        }

        [Test]
        public void ParseDomainFile_ValidFile_ParsesDomain()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var filePath = "test-domain.pddl"; // Will be created in setup


            var result = parser.ParseDomainFile(filePath);


            if (!result.Success)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Message));
                Assert.Fail($"Parse failed with errors: {errors}");
            }
            Assert.That(result.Success, Is.True);
            Assert.That(result.Result, Is.Not.Null);
        }
    }
}
