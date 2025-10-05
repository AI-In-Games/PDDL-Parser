#!/bin/bash
# Build Unity Package from Source
# This script copies the necessary files from src/PDDLParser to UnityPackage/Runtime

set -e

echo "Building Unity Package..."

# Clean existing Runtime directory (preserve asmdef)
if [ -f "UnityPackage/Runtime/AIInGames.Planning.PDDL.asmdef" ]; then
    cp UnityPackage/Runtime/AIInGames.Planning.PDDL.asmdef /tmp/pddl-asmdef-backup.json
fi
rm -rf UnityPackage/Runtime

# Create directory structure
mkdir -p UnityPackage/Runtime/Generated
mkdir -p UnityPackage/Runtime/Plugins

# Restore asmdef
if [ -f "/tmp/pddl-asmdef-backup.json" ]; then
    cp /tmp/pddl-asmdef-backup.json UnityPackage/Runtime/AIInGames.Planning.PDDL.asmdef
fi

# Copy source directories (exclude build artifacts and grammar)
for dir in src/PDDLParser/*/; do
    dirname=$(basename "$dir")
    if [[ "$dirname" != "bin" && "$dirname" != "obj" && "$dirname" != "Grammar" && "$dirname" != "Generated" ]]; then
        cp -r "$dir" "UnityPackage/Runtime/"
    fi
done

# Copy root source files
cp src/PDDLParser/*.cs UnityPackage/Runtime/ 2>/dev/null || true

# Copy generated ANTLR files (excluding AssemblyInfo)
find src/PDDLParser/Generated -name "*.cs" ! -name "*AssemblyInfo.cs" -exec cp {} UnityPackage/Runtime/Generated/ \;

# Copy ANTLR runtime DLL from NuGet cache
ANTLR_DLL="$HOME/.nuget/packages/antlr4.runtime.standard/4.13.1/lib/netstandard2.0/Antlr4.Runtime.Standard.dll"
if [ -f "$ANTLR_DLL" ]; then
    cp "$ANTLR_DLL" UnityPackage/Runtime/Plugins/
else
    echo "Error: ANTLR Runtime DLL not found. Run 'dotnet restore' first."
    exit 1
fi

# Copy LICENSE
cp LICENSE UnityPackage/LICENSE.md

echo "Unity Package built successfully!"
echo "Package location: UnityPackage/"
