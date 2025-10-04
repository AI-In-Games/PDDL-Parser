# Build Unity Package from Source
# This script copies the necessary files from src/PDDLParser to UnityPackage/Runtime

$ErrorActionPreference = "Stop"

Write-Host "Building Unity Package..." -ForegroundColor Cyan

# Clean existing Runtime directory (preserve asmdef)
$asmdefPath = "UnityPackage/Runtime/AIInGames.Planning.PDDL.asmdef"
$asmdefBackup = "$env:TEMP\pddl-asmdef-backup.json"
if (Test-Path $asmdefPath) {
    Copy-Item $asmdefPath $asmdefBackup -Force
}
if (Test-Path "UnityPackage/Runtime") {
    Remove-Item "UnityPackage/Runtime" -Recurse -Force
}

# Create directory structure
New-Item -ItemType Directory -Path "UnityPackage/Runtime" -Force | Out-Null
New-Item -ItemType Directory -Path "UnityPackage/Runtime/Generated" -Force | Out-Null
New-Item -ItemType Directory -Path "UnityPackage/Runtime/Plugins" -Force | Out-Null

# Restore asmdef
if (Test-Path $asmdefBackup) {
    Copy-Item $asmdefBackup $asmdefPath -Force
}

# Copy source directories
$sourceDirs = @("Errors", "Implementation", "Models", "Visitors")
foreach ($dir in $sourceDirs) {
    Copy-Item "src/PDDLParser/$dir" "UnityPackage/Runtime/$dir" -Recurse -Force
}

# Copy root source files (excluding csproj)
Get-ChildItem "src/PDDLParser/*.cs" | Copy-Item -Destination "UnityPackage/Runtime/" -Force

# Copy generated ANTLR files (excluding AssemblyInfo)
Get-ChildItem "src/PDDLParser/Generated/*.cs" | Where-Object { $_.Name -ne "PDDLParser.AssemblyInfo.cs" } | Copy-Item -Destination "UnityPackage/Runtime/Generated/" -Force

# Copy ANTLR runtime DLL from NuGet cache
$antlrDll = "$env:USERPROFILE\.nuget\packages\antlr4.runtime.standard\4.13.1\lib\netstandard2.0\Antlr4.Runtime.Standard.dll"
if (Test-Path $antlrDll) {
    Copy-Item $antlrDll "UnityPackage/Runtime/Plugins/" -Force
} else {
    Write-Error "ANTLR Runtime DLL not found. Run 'dotnet restore' first."
}

# Copy LICENSE
Copy-Item "LICENSE" "UnityPackage/LICENSE.md" -Force

Write-Host "Unity Package built successfully!" -ForegroundColor Green
Write-Host "Package location: UnityPackage/" -ForegroundColor Yellow
