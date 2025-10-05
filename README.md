# PDDL Parser for C# and Unity

A PDDL (Planning Domain Definition Language) parser for C# and Unity, supporting STRIPS, typing, negative preconditions, conditional effects, and basic ADL features.

**Current Version:** 0.0.1 (Initial release - parsing only)
**Next Milestone:** 0.1.0 (Adds PDDL serialization with round-trip support)

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

Contact: aiingames@hotmail.com or [open an issue](https://github.com/AI-In-Games/PDDL-Parser/issues)

## Contributing

Contributions are welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.
