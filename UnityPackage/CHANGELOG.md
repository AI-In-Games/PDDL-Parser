# Changelog

All notable changes to this package will be documented in this file.

## [Unreleased]

### Added
- PDDL serialization with round-trip support: parse PDDL → modify in code → serialize back to PDDL via `ToPddl()` methods ([#2](https://github.com/AI-In-Games/PDDL-Parser/issues/2))

## [0.0.1] - 2025-10-04

Initial release.

### Added
- PDDL domain and problem parsing
- Support for STRIPS, typing, negative preconditions, and conditional effects
- Universal and existential quantification (forall, exists)
- Unity 2021.2+ compatibility (.NET Standard 2.1)
- Pre-generated ANTLR files (no build-time dependencies)
- ANTLR runtime included as precompiled DLL
