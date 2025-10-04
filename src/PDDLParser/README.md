# PDDLParser Source

Version: 0.0.1

This directory contains the PDDL parser library implementation.

## Project Structure

```
PDDLParser/
├── Models/           # Domain model interfaces (IDomain, IProblem, IAction, etc.)
├── Internal/         # Concrete implementations of model interfaces
├── Visitors/         # ANTLR visitor pattern implementations
├── Errors/           # Error handling and reporting
├── Grammar/          # ANTLR grammar files (Pddl.g4)
├── Generated/        # Pre-generated ANTLR files for Unity import
├── IPDDLParser.cs    # Main parser interface
├── IParseResult.cs   # Parse result wrapper
└── PDDLParser.cs     # Parser implementation
```

## ANTLR Code Generation

The parser uses ANTLR 4 to generate lexer and parser code from the grammar file. Generated files are placed in two locations:

1. `obj/Debug/netstandard2.1/` - Used during .NET compilation
2. `Generated/` - Pre-generated files for Unity import (no ANTLR dependency needed)

The build process automatically cleans local file paths from generated files to keep them portable.

## Adding PDDL Features

To extend the parser with new PDDL features:

1. Update `Grammar/Pddl.g4` with new grammar rules
2. Run `dotnet build` to regenerate parser files
3. Add corresponding model interfaces in `Models/`
4. Update visitor implementations in `Visitors/`
5. Add tests in the test project

## Dependencies

- ANTLR 4.13.1 runtime (for .NET builds)
- .NET Standard 2.1

For Unity builds, no runtime dependencies are needed since parser code is pre-generated.
