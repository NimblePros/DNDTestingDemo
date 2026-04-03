# Property-Based Demo Tests

This folder contains property-based testing examples for the D&D themed demo using FsCheck and xUnit.

## What this contains

- `PropertyBasedTestingExamples.cs`: FsCheck property tests demonstrating:
  - Custom generator registration with `DndArbitraries`
  - Custom generators for characters, enemies, parties, encounters
  - Shrinker behavior and domain-aware shrinker examples
  - Generator-driven assertions for D&D domain invariants

- `Arbitraries.cs`: Maps custom `DndGenerators` into FsCheck `Arbitrary<T>` types.

- `DndGenerators.cs`: Domain-style generator definitions in `src/property-based-demo/PropertyBasedDemo`.

## Run the tests

Use this command from solution root (`d:\code\NimblePros\DndTestingDemo`):

```bash
dotnet test tests/property-based-demo/PropertyBasedDemo.Tests/PropertyBasedDemo.Tests.csproj --logger "console;verbosity=minimal"
```

## Notes

- Failing assertions in property-based tests are expected and useful to demonstrate shrinking behavior.
- If you want a quick pass mode, adjust the property constraints or the generator ranges to align with expectations.
