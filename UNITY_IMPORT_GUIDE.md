# Unity Import Guide

This guide explains how to import the PDDL Parser into Unity.

## Prerequisites

- Unity 2021.2 or later (for .NET Standard 2.1 support)

## Installation

### Unity Package Manager (Recommended)

1. Open Unity Package Manager (Window > Package Manager)
2. Click the '+' button in the top-left corner
3. Select "Add package from git URL..."
4. Enter: `https://github.com/AI-In-Games/PDDL-Parser.git?path=/UnityPackage`
5. Click "Add"

Unity will automatically download and install the package with all dependencies (ANTLR runtime DLL included).

**What's included:**
- All parser source code
- Pre-generated ANTLR files
- ANTLR Runtime DLL (188KB)
- Assembly definition for seamless integration

**Updates:** Use Package Manager to update to newer versions when available.

### Manual Import (Advanced)

**Note:** Only use this method if you need to modify the parser source code or cannot use Unity Package Manager.

#### Step 1: Copy Source Files to Unity

Copy these folders from `src/PDDLParser/` to your Unity project (e.g., `Assets/Plugins/PDDLParser/` or `Assets/Scripts/Planning/`):

```
Your Unity Project/Assets/Plugins/PDDLParser/
├── Models/                    # All interface files (*.cs)
├── Implementation/            # Implementation classes (*.cs)
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

#### Step 2: Verify Generated Files

The `Generated/` folder should contain these 4 pre-generated C# files:
- `PddlLexer.cs`
- `PddlParser.cs`
- `PddlBaseVisitor.cs`
- `PddlVisitor.cs`

These files are **auto-generated** from the grammar and contain the correct namespace `AIInGames.Planning.PDDL.Generated`.

#### Step 3: Add ANTLR Runtime DLL

1. Download ANTLR Runtime DLL from NuGet or copy from your local cache:
   - Path: `~/.nuget/packages/antlr4.runtime.standard/4.13.1/lib/netstandard2.0/Antlr4.Runtime.Standard.dll`
   - Or download from: https://www.nuget.org/packages/Antlr4.Runtime.Standard/

2. Copy the DLL to your Unity project:
   - Recommended location: `Assets/Plugins/Antlr4.Runtime.Standard.dll`

#### Step 4: Verify in Unity

1. Open your Unity project
2. Check the Console for any compilation errors
3. The parser should now compile successfully

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

## Dependencies

This import method requires:
- ANTLR Runtime DLL (Antlr4.Runtime.Standard.dll - 188KB, .NET Standard 2.0)
- No other external dependencies

The DLL works on all Unity platforms including IL2CPP, WebGL, iOS, Android, Nintendo Switch, and consoles.

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
