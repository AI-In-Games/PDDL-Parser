# PDDL Parser for C# and Unity

A PDDL (Planning Domain Definition Language) parser for C# and Unity, supporting STRIPS, typing, negative preconditions, conditional effects, and basic ADL features.

**Current Version:** 0.0.1 (Initial release - parsing only)
**Next Milestone:** [0.1.0](https://github.com/AI-In-Games/PDDL-Parser/milestone/1) (Adds PDDL serialization with round-trip support)

[Changelog](CHANGELOG.md) | [Contributing](CONTRIBUTING.md)

## Installation

### For .NET Projects

Install via NuGet (once published):

```bash
dotnet add package AIInGames.Planning.PDDL
```

### For Unity Projects

#### Option 1: Unity Package Manager (Recommended)

1. Open Unity Package Manager (Window > Package Manager)
2. Click the '+' button in the top-left corner
3. Select "Add package from git URL..."
4. Enter: `https://github.com/AI-In-Games/PDDL-Parser.git?path=/UnityPackage`

The package includes pre-generated ANTLR files and runtime DLL - everything is self-contained.

#### Option 2: From Source

Copy the source files from `src/PDDLParser/` directly into your Unity project. See [UNITY_IMPORT_GUIDE.md](UNITY_IMPORT_GUIDE.md) for detailed instructions.

## Usage

All parse methods return `IParseResult<T>`, which carries either the parsed object or a list of errors with line and column numbers.

### Parsing from a string

```csharp
using AIInGames.Planning.PDDL;

var parser = new PDDLParser();

var domainResult = parser.ParseDomain(domainText);
if (domainResult.Success)
{
    var domain = domainResult.Result;
}
else
{
    foreach (var error in domainResult.Errors)
        Console.WriteLine($"Line {error.Line}, col {error.Column}: {error.Message}");
}

var problemResult = parser.ParseProblem(problemText);
if (problemResult.Success)
{
    var problem = problemResult.Result;
}
```

### Parsing from a file

```csharp
var domainResult = parser.ParseDomainFile("path/to/domain.pddl");
var problemResult = parser.ParseProblemFile("path/to/problem.pddl");
```

## Reading Parsed Data

After parsing, the domain and problem objects expose all PDDL constructs through typed interfaces.

### Domain data

```csharp
var domain = parser.ParseDomain(domainText).Result!;

Console.WriteLine(domain.Name); // "blocksworld"

foreach (var predicate in domain.Predicates)
    Console.WriteLine($"{predicate.Name}/{predicate.Arity}"); // e.g. "on/2", "clear/1"

foreach (var action in domain.Actions)
{
    Console.WriteLine(action.Name); // e.g. "pick-up"

    foreach (var param in action.Parameters)
        Console.WriteLine($"  ?{param.Name} - {param.Type?.Name ?? "object"}");

    // Precondition is a tree - root is usually ConditionType.And
    var pre = action.Precondition;
    if (pre.Type == ConditionType.And)
    {
        foreach (var child in pre.Children)
            if (child.Type == ConditionType.Literal)
                Console.WriteLine($"  requires: {child.Literal!.Predicate.Name}");
    }
}

// Typed lookups return null if not found
var pickUp = domain.GetAction("pick-up");
var onPredicate = domain.GetPredicate("on");
```

### Problem data

```csharp
var problem = parser.ParseProblem(problemText).Result!;

Console.WriteLine(problem.Name);       // "blocks-4"
Console.WriteLine(problem.DomainName); // "blocksworld"

foreach (var obj in problem.Objects)
    Console.WriteLine($"{obj.Name} - {obj.Type?.Name ?? "object"}");

// Initial state is a flat list of ground literals
foreach (var literal in problem.InitialState)
    Console.WriteLine($"{(literal.IsNegated ? "not " : "")}{literal.Predicate.Name}({string.Join(", ", literal.Arguments)})");

// Goal is a condition tree (same structure as action preconditions)
var goal = problem.Goal;
if (goal.Type == ConditionType.And)
    Console.WriteLine($"Goal has {goal.Children.Count} conjuncts");
```

## Plan and Domain Validation (optional)

The optional `AIInGames.Planning.PDDL.Validation` assembly validates domains, problems, and
plans using the external [VAL](https://github.com/KCL-Planning/VAL) tool. It is gated behind
the `ENABLE_VALIDATION` compile symbol so it is only included when you opt in.

- **.NET:** reference the `PDDLParser.Validation` project/package. It is built with
  `ENABLE_VALIDATION` defined by default; disable with `-p:EnableValidation=false`.
- **Unity:** add `ENABLE_VALIDATION` to Project Settings > Player > Scripting Define Symbols.
  The validation assembly only compiles when the define is present.

```csharp
using AIInGames.Planning.PDDL.Validation;

var validator = new ValPlanValidator();

// Validate a domain against a problem (type-checking, predicate arities, goal well-formedness)
ValidationResult result = validator.ValidateDomainAndProblem(domainPddl, problemPddl);

// Validate a plan (one grounded action per line: "(action arg1 arg2)")
ValidationResult planResult = validator.ValidatePlan(domainPddl, problemPddl, planPddl);

if (planResult.BinaryMissing)
    Console.WriteLine("VAL binary not available for this platform.");
else if (planResult.IsValid)
    Console.WriteLine("Plan is valid.");
else
    foreach (var error in planResult.Errors)
        Console.WriteLine(error);
```

Overloads accepting parsed `IDomain`/`IProblem` are also provided (they serialize via
`ToPddl()`). The package bundles the Windows (`win64`) VAL binary; see
`Binaries/BUILD.md` for building the Linux and macOS binaries. The validator runs VAL as an
external process, so it requires a platform that supports `Process.Start` (desktop/editor;
not IL2CPP/console player builds).

## Supported PDDL Features

- STRIPS (basic actions, preconditions, effects)
- Typing
- Negative preconditions
- Conditional effects
- Universal and existential quantification (forall, exists)
- Basic ADL (and, or, not, imply)

## Building

```bash
dotnet build
dotnet test
```

## Unity Compatibility

Targets .NET Standard 2.1, compatible with Unity 2021.2 and later. The Unity package includes pre-generated parser files and the ANTLR runtime DLL (self-contained, no additional downloads needed).

## License

MIT License. See LICENSE file for details.

## Acknowledgments

This project uses ANTLR 4 for parsing (BSD 3-Clause License, Copyright 2012-2017 The ANTLR Project).

The PDDL grammar is based on work by Zeyn Saigol (School of Computer Science, University of Birmingham) and the ANTLR v4 port by Tom Everett, available in the ANTLR grammars-v4 repository at https://github.com/antlr/grammars-v4/tree/master/pddl.

Testing uses NUnit (MIT License, Copyright 2021 Charlie Poole, Rob Prouse).

## Author

Created by Marek Marchlewicz
Published by [AI In Games](https://aiingames.com)

Contact: contact@aiingames.com or [open an issue](https://github.com/AI-In-Games/PDDL-Parser/issues)

## Contributing

Contributions are welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.
