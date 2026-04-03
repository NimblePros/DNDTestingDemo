# Contract Testing Demo with NSwag

This demo showcases API client generation using NSwag from an OpenAPI specification, demonstrating both consumer and provider contract testing approaches.

## Story

In the D&D world, the **Bard Service** needs to request a "Masterwork Sword" from the **Blacksmith Service**. The contract between these services is defined by the OpenAPI specification (`swagger.json`), which ensures both services agree on the API structure.

## What This Demo Shows

- **OpenAPI Specification**: Defines the Blacksmith Service API contract
- **NSwag Client Generation**: Automatically generates a type-safe C# client from the OpenAPI spec
- **Consumer Testing**: Tests that the client (consumer) correctly calls and parses provider responses
- **Provider Testing**: Tests that the API implementation (provider) fulfills the contract

## Test Types

### Consumer Test
Tests the **Bard Service** (consumer) to ensure it can correctly interact with the Blacksmith Service API. Uses a mock HTTP handler to simulate provider responses.

### Provider Test
Tests the **Blacksmith Service** (provider) to ensure its actual API implementation matches the OpenAPI specification. Uses TestServer to run the real API.

## Running the Demo

```bash
dotnet test tests/contract-testing/ContractTestingDemo.Tests/ContractTestingDemo.Tests.csproj --logger "console;verbosity=minimal"
```

## Files

- `swagger.json` - OpenAPI 3.0 specification for the Blacksmith Service
- `BlacksmithClient.cs` - NSwag-generated C# client
- `BardBlacksmithContractTests.cs` - Consumer and provider tests

## Key Concepts

- **Contract as Code**: The OpenAPI spec serves as the contract between services
- **Type-Safe Clients**: NSwag generates strongly-typed clients from specifications
- **Dual Perspective Testing**: Both consumer and provider perspectives ensure contract compliance
- **Mock vs. Real Testing**: Consumer tests use mocks; provider tests use actual implementations

This approach provides comprehensive contract testing while maintaining simplicity compared to full Pact implementations.