# Contributing to PDDL Parser

Thank you for contributing. To get started:

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

## Testing Your Changes

Before submitting a PR:

1. **Run tests:**
   ```bash
   dotnet test
   ```

2. **Build in Release mode:**
   ```bash
   dotnet build --configuration Release
   ```

3. **Rebuild Unity package:**
   ```bash
   ./build-unity-package.sh  # or .ps1 on Windows
   ```

4. **Commit all changes:**
   - Include source code changes
   - Include generated Unity package files in `UnityPackage/Runtime/`

## Pull Request Guidelines

- Keep PRs focused on a single feature or fix
- Include tests for new functionality
- Update documentation if needed
- All tests must pass
- Follow existing code style

---

## For Maintainers Only

<details>
<summary>Release Process (click to expand)</summary>

### Releasing a New Version

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

3. Commit and push to `main`:
   ```bash
   git add .
   git commit -m "Release v0.0.1"
   git push origin main
   ```

4. **Publish to NuGet (Manual):**
   - Go to: https://github.com/AI-In-Games/PDDL-Parser/actions/workflows/publish-nuget.yml
   - Click "Run workflow"
   - Select branch: `main`
   - Click "Run workflow"

5. **Create GitHub Release (Optional):**
   - Create and push a version tag:
     ```bash
     git tag v0.0.1
     git push origin v0.0.1
     ```
   - This auto-creates a GitHub release with downloadable ZIPs

</details>
