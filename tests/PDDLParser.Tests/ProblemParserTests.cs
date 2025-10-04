using AIInGames.Planning.PDDL;
using NUnit.Framework;

namespace AIInGames.Planning.PDDL.Tests
{
    [TestFixture]
    public class ProblemParserTests
    {
        private const string SimpleProblem = @"
(define (problem blocks-4)
  (:domain blocksworld)
  (:objects a b c d)
  (:init (clear c)
         (clear a)
         (clear b)
         (clear d)
         (ontable c)
         (ontable a)
         (ontable b)
         (ontable d)
         (handempty))
  (:goal (and (on d c)
              (on c b)
              (on b a)))
)";

        private const string TypedProblem = @"
(define (problem log-x-1)
  (:domain logistics)
  (:objects
    truck1 - truck
    package1 package2 - package
    location1 location2 - location)
  (:init (at package1 location1)
         (at package2 location1)
         (at-truck truck1 location1))
  (:goal (and (at package1 location2)
              (at package2 location2)))
)";

        [Test]
        public void ParseProblem_SimpleProblem_ReturnsProblem()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(SimpleProblem);


            Assert.That(result.Success, Is.True, "Parsing should succeed");
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result!.Name, Is.EqualTo("blocks-4"));
        }

        [Test]
        public void ParseProblem_SimpleProblem_ParsesDomainName()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(SimpleProblem);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.DomainName, Is.EqualTo("blocksworld"));
        }

        [Test]
        public void ParseProblem_SimpleProblem_ParsesObjects()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(SimpleProblem);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Objects.Count, Is.EqualTo(4));

            var objA = result.Result!.GetObject("a");
            Assert.That(objA, Is.Not.Null);
            Assert.That(objA.Name, Is.EqualTo("a"));
        }

        [Test]
        public void ParseProblem_SimpleProblem_ParsesInitialState()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(SimpleProblem);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.InitialState, Is.Not.Empty);
            Assert.That(result.Result!.InitialState.Count, Is.EqualTo(9));
        }

        [Test]
        public void ParseProblem_SimpleProblem_ParsesGoal()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(SimpleProblem);


            Assert.That(result.Success, Is.True);
            Assert.That(result.Result!.Goal, Is.Not.Null);
            Assert.That(result.Result!.Goal.Type, Is.EqualTo(ConditionType.And));
            Assert.That(result.Result!.Goal.Children.Count, Is.EqualTo(3));
        }

        [Test]
        public void ParseProblem_TypedProblem_ParsesTypedObjects()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();


            var result = parser.ParseProblem(TypedProblem);


            Assert.That(result.Success, Is.True);

            var truck = result.Result!.GetObject("truck1");
            Assert.That(truck, Is.Not.Null);
            Assert.That(truck.Type?.Name, Is.EqualTo("truck"));

            var package1 = result.Result!.GetObject("package1");
            Assert.That(package1, Is.Not.Null);
            Assert.That(package1.Type?.Name, Is.EqualTo("package"));
        }

        [Test]
        public void ParseProblem_InvalidSyntax_ReturnsErrors()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var invalidProblem = "(define (problem test) (:domain test) missing-closing";


            var result = parser.ParseProblem(invalidProblem);


            Assert.That(result.Success, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        }

        [Test]
        public void ParseProblemFile_ValidFile_ParsesProblem()
        {

            IPDDLParser parser = new global::AIInGames.Planning.PDDL.PDDLParser();
            var filePath = "test-problem.pddl"; // Will be created in setup


            var result = parser.ParseProblemFile(filePath);


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
