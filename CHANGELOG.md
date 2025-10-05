# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- PDDL serialization with round-trip support: parse PDDL → modify in code → serialize back to PDDL via `ToPddl()` methods ([#2](https://github.com/AI-In-Games/PDDL-Parser/issues/2))

## [0.0.1] - 2025-10-04

Initial release.

### Added
- PDDL domain and problem parsing
- Support for STRIPS, typing, negative preconditions, and conditional effects
- Universal and existential quantification (forall, exists)
- .NET Standard 2.1 target for Unity 2021.2+ compatibility
- NuGet package distribution
- Pre-generated ANTLR files and runtime DLL for Unity Package Manager integration
- Comprehensive test suite with NUnit
- GitHub Actions for CI/CD and automated releases
