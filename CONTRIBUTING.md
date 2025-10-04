# Contributing to PDDL Parser

Thanks for your interest in contributing! Here's how to get started.

## Quick Start

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes in `src/PDDLParser/`
4. Run tests: `dotnet test`
5. Rebuild Unity package: `./build-unity-package.sh`
6. Commit your changes (include both source and generated Unity package files)
7. Push to your fork and open a pull request

## Development Workflow

### Building the Unity Package

The `UnityPackage/` directory is **auto-generated** from the source code. Do not manually edit files in `UnityPackage/Runtime/` - they will be overwritten.

To rebuild the Unity package after making changes:

**Windows:**
```powershell
.\build-unity-package.ps1
```

**Linux/macOS:**
```bash
./build-unity-package.sh
```

The build script:
1. Cleans the `UnityPackage/Runtime/` directory
2. Copies source files from `src/PDDLParser/`
3. Copies generated ANTLR files from `src/PDDLParser/Generated/`
4. Copies ANTLR runtime DLL from NuGet cache
5. Copies LICENSE file

### GitHub Actions

The Unity package is automatically built and verified on every push/PR via GitHub Actions. The CI ensures the Unity package stays in sync with the source code.

## Making Changes

1. Edit files in `src/PDDLParser/`
2. Run tests: `dotnet test`
3. Build Unity package: `./build-unity-package.sh` (or `.ps1` on Windows)
4. Commit both source changes and generated Unity package files

**Important**: The generated Unity package files in `UnityPackage/Runtime/` must be committed to git. This allows users to install the package directly via Unity Package Manager without needing to build it themselves.

## Project Structure

```
PDDL-Parser/
├── src/PDDLParser/          # Main source code (.NET)
│   ├── Generated/           # ANTLR generated files (committed for Unity)
│   └── ...
├── tests/                   # NUnit tests
├── UnityPackage/            # Unity Package Manager distribution
│   ├── package.json         # Package manifest (edit manually)
│   ├── README.md            # Unity-specific readme (edit manually)
│   ├── CHANGELOG.md         # Unity changelog (edit manually)
│   ├── LICENSE.md           # Auto-copied from LICENSE
│   └── Runtime/             # AUTO-GENERATED - do not edit manually!
└── build-unity-package.*    # Build scripts
```

## Release Process

1. Update version in:
   - `src/PDDLParser/PDDLParser.csproj` (NuGet version)
   - `UnityPackage/package.json` (Unity package version)
   - `CHANGELOG.md` (main changelog)
   - `UnityPackage/CHANGELOG.md` (Unity changelog)

2. Build and test:
   ```bash
   dotnet build --configuration Release
   dotnet test
   ./build-unity-package.sh
   ```

3. Commit and push to `main`

4. GitHub Actions will run CI

5. For NuGet: Manually publish the package using the GitHub release workflow
