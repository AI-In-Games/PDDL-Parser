# NuGet Publishing Setup

## One-Time Setup

### 1. Add NuGet API Key to GitHub

1. Go to your repository settings:
   - https://github.com/AI-In-Games/PDDL-Parser/settings/secrets/actions

2. Click **"New repository secret"**

3. Enter the following:
   - **Name:** `NUGET_API_KEY`
   - **Secret:** [Paste your NuGet API key here]

4. Click **"Add secret"**

That's it! You only need to do this once.

## How to Publish to NuGet

### Manual Publishing (Recommended)

1. Make sure all changes are committed and pushed to `main`

2. Go to GitHub Actions:
   - https://github.com/AI-In-Games/PDDL-Parser/actions/workflows/publish-nuget.yml

3. Click the **"Run workflow"** button (top right)

4. Select:
   - Branch: `main`
   - Click **"Run workflow"**

5. Wait for the workflow to complete (usually 2-3 minutes)

6. Check NuGet.org:
   - Your package will appear at: https://www.nuget.org/packages/AIInGames.Planning.PDDL/

### What the Workflow Does

1. Builds the project in Release mode
2. Runs all tests
3. Packs the NuGet package
4. Publishes to NuGet.org (if tests pass)
5. Uploads the package as an artifact for your records

### Troubleshooting

**"Package already exists" error:**
- The workflow automatically skips duplicates with `--skip-duplicate`
- If you need to republish the same version, increment the version number in `PDDLParser.csproj`

**"Unauthorized" error:**
- Check that your NuGet API key is still valid
- Update the GitHub secret if needed

**Tests failing:**
- Fix the tests first
- The workflow won't publish if tests fail

## Version Management

Before publishing, update version numbers in:
- `src/PDDLParser/PDDLParser.csproj` - `<Version>0.0.1</Version>`
- `UnityPackage/package.json` - `"version": "0.0.1"`
- `CHANGELOG.md` and `UnityPackage/CHANGELOG.md`
