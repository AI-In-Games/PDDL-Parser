# Unity Import Guide

This guide explains how to import the PDDL Parser into Unity **without any dependencies** using pre-generated ANTLR files.

## Prerequisites

- Unity 2021.2 or later (for .NET Standard 2.1 support)
- Git (optional, for cloning)

## Import Steps

### 1. Copy Source Files to Unity

Copy these folders from `src/PDDLParser/` to your Unity project (e.g., `Assets/Plugins/PDDLParser/` or `Assets/Scripts/Planning/`):

```
Your Unity Project/Assets/Plugins/PDDLParser/
├── Models/                    # All interface files (*.cs)
├── Internal/                  # Implementation classes (*.cs)
├── Visitors/                  # Visitor implementations (*.cs)
├── Errors/                    # Error handling classes (*.cs)
├── Generated/                 # Pre-generated ANTLR files (*.cs)
├── IPDDLParser.cs
├── IParseResult.cs
└── PDDLParser.cs
```

**DO NOT COPY:**
- `Grammar/` folder (contains .g4 file, not needed)
- `bin/` or `obj/` folders
- `.csproj` file
- Any NuGet packages

### 2. Verify Generated Files

The `Generated/` folder should contain these 4 pre-generated C# files:
- `PddlLexer.cs`
- `PddlParser.cs`
- `PddlBaseVisitor.cs`
- `PddlVisitor.cs`

These files are **auto-generated** from the grammar and contain the correct namespace `AIInGames.Planning.PDDL.Generated`.

### 3. Verify in Unity

1. Open your Unity project
2. Check the Console for any compilation errors
3. The parser should compile without any dependencies!

## Usage in Unity

```csharp
using UnityEngine;
using AIInGames.Planning.PDDL;

public class PDDLExample : MonoBehaviour
{
    void Start()
    {
        // Create parser
        var parser = new PDDLParser();

        // Load PDDL from Resources
        var domainText = Resources.Load<TextAsset>("PDDL/blocksworld-domain").text;
        var result = parser.ParseDomain(domainText);

        if (result.Success)
        {
            Debug.Log($"Loaded domain: {result.Result.Name}");
            Debug.Log($"Actions: {result.Result.Actions.Count}");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Debug.LogError($"Parse error at {error.Line}:{error.Column} - {error.Message}");
            }
        }
    }
}
```

## Updating Generated Files

If you modify the grammar (`Pddl.g4`), regenerate the files:

1. On your development machine (with .NET SDK):
   ```bash
   cd src/PDDLParser
   dotnet build
   ```

2. The build automatically copies updated files to `src/PDDLParser/Generated/`

3. Copy the new `Generated/` folder to Unity

## No Dependencies

This import method has zero external dependencies:
- No ANTLR runtime DLL needed
- No NuGet packages required
- No native plugins
- Pure C# source code
- Works on all Unity platforms
- IL2CPP compatible

The ANTLR runtime code is embedded in the generated files, so everything just works.

## Troubleshooting

### "Type or namespace 'AIInGames' could not be found"
- Make sure all folders are copied correctly
- Check that generated files are in the `Generated/` folder
- Verify Unity is using .NET Standard 2.1 (Edit > Project Settings > Player > Api Compatibility Level)

### "PddlParser not found in namespace"
- Check that `PDDLParser.cs` is in the root of your import folder
- Verify the namespace is `AIInGames.Planning.PDDL`

### Assembly definition conflicts
- If you're using assembly definitions (`.asmdef`), make sure the PDDL parser folder is either:
  - Outside any `.asmdef` scope (will be in default assembly)
  - OR has its own `.asmdef` that other scripts reference

## Platform Compatibility

Unity Version Requirements:
- Unity 2021.2+ (.NET Standard 2.1)
- Unity 2020.x may work with .NET Standard 2.0 compatibility

## License

See [LICENSE](LICENSE) for licensing information. Acknowledgments are included in the main [README.md](README.md).
