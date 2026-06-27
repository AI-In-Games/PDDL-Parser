#if ENABLE_VALIDATION
using AIInGames.Planning.PDDL.Validation;
using NUnit.Framework;

namespace AIInGames.Planning.PDDL.Tests.Validation
{
    [TestFixture]
    public class ValPlanValidatorTests
    {
        private const string ValidDomain = @"
(define (domain soldier)
  (:requirements :strips :typing :negative-preconditions)
  (:types location agent)
  (:predicates
    (at ?a - agent ?l - location)
    (adjacent ?from - location ?to - location)
    (armed ?a - agent))
  (:action move
    :parameters (?a - agent ?from - location ?to - location)
    :precondition (and (at ?a ?from) (adjacent ?from ?to))
    :effect (and (at ?a ?to) (not (at ?a ?from))))
  (:action shoot
    :parameters (?a - agent ?l - location)
    :precondition (and (at ?a ?l) (armed ?a))
    :effect (not (armed ?a)))
)";

        private const string ValidProblem = @"
(define (problem soldier-p)
  (:domain soldier)
  (:objects
    hero - agent
    base camp - location)
  (:init
    (at hero base)
    (adjacent base camp)
    (armed hero))
  (:goal (at hero camp))
)";

        private const string DomainWithTypeError = @"
(define (domain bad)
  (:requirements :strips :typing)
  (:types agent)
  (:predicates (armed ?a - agent))
  (:action shoot
    :parameters (?a - nonexistenttype)
    :precondition (armed ?a)
    :effect (not (armed ?a)))
)";

        private const string MinimalProblemForBadDomain = @"
(define (problem bad-p)
  (:domain bad)
  (:objects dummy - agent)
  (:init)
  (:goal (and))
)";

        private const string ValidPlan = "(move hero base camp)";

        // Precondition (at hero camp) is false in the initial state
        private const string InvalidPlan = "(move hero camp base)";

        private static void SkipIfBinaryMissing(ValidationResult result)
        {
            if (result.BinaryMissing)
                Assert.Ignore("Validate binary not present for this platform; skipping VAL integration test.");
        }

        [Test]
        public void ValidateDomainAndProblem_ValidDomainAndProblem_ReturnsValid()
        {
            ValidationResult result = ValPlanValidator.ValidateDomainAndProblem(ValidDomain, ValidProblem);

            SkipIfBinaryMissing(result);
            Assert.IsTrue(result.IsValid, $"Expected valid but got errors:\n{string.Join("\n", result.Errors)}");
        }

        [Test]
        public void ValidateDomainAndProblem_DomainWithUndefinedType_ReturnsInvalid()
        {
            ValidationResult result = ValPlanValidator.ValidateDomainAndProblem(DomainWithTypeError, MinimalProblemForBadDomain);

            SkipIfBinaryMissing(result);
            Assert.IsFalse(result.IsValid, "Domain with undefined type should fail validation");
            Assert.IsTrue(result.Errors.Count > 0, "Should report at least one error");
        }

        [Test]
        public void ValidateDomainAndProblem_ValidDomain_RawOutputIsNotEmpty()
        {
            ValidationResult result = ValPlanValidator.ValidateDomainAndProblem(ValidDomain, ValidProblem);

            SkipIfBinaryMissing(result);
            Assert.IsNotNull(result.RawOutput);
            Assert.IsNotEmpty(result.RawOutput);
        }

        [Test]
        public void ValidatePlan_ValidPlan_ReturnsValid()
        {
            ValidationResult result = ValPlanValidator.ValidatePlan(ValidDomain, ValidProblem, ValidPlan);

            SkipIfBinaryMissing(result);
            Assert.IsTrue(result.IsValid, $"Expected valid but got:\n{result.RawOutput}");
        }

        [Test]
        public void ValidatePlan_PlanWithUnsatisfiedPrecondition_ReturnsInvalid()
        {
            ValidationResult result = ValPlanValidator.ValidatePlan(ValidDomain, ValidProblem, InvalidPlan);

            SkipIfBinaryMissing(result);
            Assert.IsFalse(result.IsValid, "Plan with unsatisfied precondition should fail validation");
        }

        [Test]
        public void ValidatePlan_RawOutputContainsPlanDetails()
        {
            ValidationResult result = ValPlanValidator.ValidatePlan(ValidDomain, ValidProblem, ValidPlan);

            SkipIfBinaryMissing(result);
            Assert.IsTrue(result.RawOutput.Contains("Plan valid"), $"Expected 'Plan valid' in output:\n{result.RawOutput}");
        }

        [Test]
        public void ValidateDomainAndProblem_ParsedModels_MatchesRawPddlResult()
        {
            IPDDLParser parser = new PDDLParser();
            var domain = parser.ParseDomain(ValidDomain).Result;
            var problem = parser.ParseProblem(ValidProblem).Result;
            Assert.That(domain, Is.Not.Null);
            Assert.That(problem, Is.Not.Null);

            ValidationResult result = ValPlanValidator.ValidateDomainAndProblem(domain!, problem!);

            SkipIfBinaryMissing(result);
            Assert.IsTrue(result.IsValid, $"Expected valid but got errors:\n{string.Join("\n", result.Errors)}");
        }

        [Test]
        public void ValidateDomainAndProblem_BinaryNotFound_ReportsBinaryMissing()
        {
            ValidationResult result = ValPlanValidator.ValidateDomainAndProblem(
                ValidDomain, ValidProblem, new MissingBinaryLocator());

            Assert.IsTrue(result.BinaryMissing, "Should report the binary as missing");
            Assert.IsFalse(result.IsValid);
        }

        private sealed class MissingBinaryLocator : IValidateBinaryLocator
        {
            public string? FindValidateExecutable() => null;
        }
    }
}
#endif
