# PDDL Parser for C# and Unity

A PDDL (Planning Domain Definition Language) parser for C# and Unity, supporting STRIPS, typing, negative preconditions, conditional effects, and basic ADL features.

Version: 0.0.1

## Installation

### For .NET Projects

Install via NuGet (once published):

```bash
dotnet add package AIInGames.Planning.PDDL
```

### From Source

Copy the source files from `src/PDDLParser/` directly into your project. This works for any C# project including Unity. See `UNITY_IMPORT_GUIDE.md` for Unity-specific instructions.

## Usage

```csharp
using AIInGames.Planning.PDDL;

var parser = new PDDLParser();

// Parse domain
var domainResult = parser.ParseDomain(domainText);
if (domainResult.Success)
{
    var domain = domainResult.Result;
    // Use domain.Actions, domain.Predicates, etc.
}

// Parse problem
var problemResult = parser.ParseProblem(problemText);
if (problemResult.Success)
{
    var problem = problemResult.Result;
    // Use problem.Objects, problem.InitialState, problem.Goal
}
```

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

Targets .NET Standard 2.1, compatible with Unity 2021.2 and later. The Unity distribution includes pre-generated parser files, so no ANTLR dependency is needed at runtime.

## License

MIT License. See LICENSE file for details.

## Acknowledgments

This project uses ANTLR 4 for parsing (BSD 3-Clause License, Copyright 2012-2017 The ANTLR Project).

The PDDL grammar is based on work by Zeyn Saigol (School of Computer Science, University of Birmingham) and the ANTLR v4 port by Tom Everett, available in the ANTLR grammars-v4 repository at https://github.com/antlr/grammars-v4/tree/master/pddl.

Testing uses NUnit (MIT License, Copyright 2021 Charlie Poole, Rob Prouse).

## Author

Created by Marek Marchlewicz
Published by [AI In Games](https://aiingames.com)

Contact: aiingames@hotmail.com or [open an issue](https://github.com/AI-In-Games/PDDL-Parser/issues)

## Contributing

Pull requests are welcome. Please include tests for new features.
