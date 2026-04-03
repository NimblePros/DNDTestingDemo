# Contract Testing Demo (Pact-Net)

This demo covers a contract between:
- Consumer: `Bard Service`
- Provider: `Blacksmith Service`

Story: Bard Service asks Blacksmith Service for a `Masterwork Sword`.
If provider returns `Sharp Stick`, contract should fail before battlefield integration.

## Tests

- `BardBlacksmithContractTests.cs`:
  - `BardService_RequestsMasterworkSword_FromBlacksmithService`: consumer contract back-end test using Pact mock server.
  - `BlacksmithService_VerifiesAgainstMasterworkSwordContract`: provider verification against generated Pact file.

## Run

From solution root:

```bash
dotnet test tests/contract-testing/ContractTestingDemo.Tests/ContractTestingDemo.Tests.csproj --logger "console;verbosity=minimal"
```

Pact file is written to: `tests/contract-testing/pacts/bard service-blacksmith service.json` (config-relative).
