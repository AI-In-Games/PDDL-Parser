# PDDL Parser for Unity

A PDDL (Planning Domain Definition Language) parser for Unity, supporting STRIPS, typing, negative preconditions, conditional effects, and basic ADL features.

**Current Version:** 0.0.1 (Initial release - parsing only)
**Next Milestone:** 0.1.0 (Adds PDDL serialization with round-trip support)

[Changelog](CHANGELOG.md)

## Installation

### Unity Package Manager

1. Open Unity Package Manager (Window > Package Manager)
2. Click the '+' button in the top-left corner
3. Select "Add package from git URL..."
4. Enter: `https://github.com/AI-In-Games/PDDL-Parser.git?path=/UnityPackage`

Note the `?path=/UnityPackage` suffix - this tells Unity to use only the UnityPackage subdirectory.

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

## Requirements

- Unity 2021.2 or later
- .NET Standard 2.1

## Dependencies

This package includes:
- Pre-generated ANTLR parser files
- ANTLR Runtime DLL (Antlr4.Runtime.Standard.dll - 188KB, .NET Standard 2.0)

Everything is self-contained - no additional downloads required. The DLL works on all Unity platforms including IL2CPP, WebGL, iOS, Android, Nintendo Switch, and consoles.

## License

MIT License - see LICENSE.md file included in this package.

This package uses ANTLR 4 (BSD 3-Clause License, Copyright 2012-2017 The ANTLR Project). The PDDL grammar is based on work by Zeyn Saigol and Tom Everett.

## Support

For issues, questions, or contributions, visit: https://github.com/AI-In-Games/PDDL-Parser
