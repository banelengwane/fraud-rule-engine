# Fraud Rule Engine Service

An ultra-fast, lightweight backend service built with C# and .NET 8 that evaluates incoming transaction events against a dynamic, configurable set of fraud rules. 

This project was built from the ground up using **Test-Driven Development (TDD)** and implements a highly decoupled architecture utilizing the **Specification Pattern** via reflection for dynamic rule execution.

---

## 🚀 Features

* **Dynamic Rule Evaluation:** Evaluate transactions at runtime using configuration profiles without changing core backend code.
* **Decoupled Specification Engine:** Uses dynamic reflection to map JSON-based property assertions to strongly typed fields.
* **Thread-Safe In-Memory Ledger:** Uses highly concurrent storage (`ConcurrentBag`) to track and log flagged transactions without performance bottlenecks.
* **100% Test-Driven:** Built completely with an automated test suite using xUnit to ensure robustness and correctness.

---

## 🛠️ Architecture Overview

The system architecture consists of two main pillars:
1.  **The Rule Engine:** Parses a JSON rule matrix configuration and checks if the property value of an incoming `Transaction` triggers a fraud threshold.
2.  **The Threat Store Repository:** A lightning-fast, thread-safe in-memory store that holds flagged transaction entries for operational review.

### Rule Configuration Structure
Rules are defined as flexible JSON payloads:
```json
[
  {
    "RuleName": "HighValueTransaction",
    "Property": "Amount",
    "Operator": "GreaterThan",
    "Value": "50000"
  }
]
```
## Prequisites
- .NET 8.0 SDK
- Terminal interface - ubuntu/Linux, macOS, or Windows WSL/Powershell
- curl or any API Client

## Project setup
1. Clone and Build solution
    - dotnet restore
    - dotnet build
2. Rnning the Test Suite
    dotnet test
3. Launching the service
    - cd /FraudEngine.Api
    - dotnet run --project FraudEngine.Api/FraudEngine.Api.csproj

## API Endpoints & Usage Examples
1. Process a transaction : Evaluates a transaction event against the active rule engine config
    - URL: /api/transactions
    - Method: POST
    - Headers: Content-Type: application/json
### Scenario A: Legitimate Transaction (Approved) 
- curl -X POST http://localhost:5243/api/transactions \
-H "Content-Type: application/json" \
-d '{"id":"tx-101","accountNumber":"ACC-001","amount":250.00,"country":"ZA","merchantType":"GroceryStore"}'
- Expected Response:
```json
{
    "status": "APPROVED"
}
```
### Scenario B: Flagged Transaction (Triggers Sanction Zone Rule)
- curl -X POST http://localhost:5243/api/transactions \
-H "Content-Type: application/json" \
-d '{"id":"tx-102","accountNumber":"ACC-002","amount":1500.00,"country":"ZA","merchantType":"CryptoExchange"}'
- Expected Response:
```json
{
  "status": "FLAGGED",
  "reason": [
    "CryptoSanctionZone"
  ]
}
```

2. Retrieved Flagged Transactions : Returns a complete ledger of all transactions flagged as potential fraud for operational review
    - URL: /api/fraud/flagged
    - Method: GET
    - Expected Response: ```json
    [
        {
            "transactionId": "tx-102",
            "amount": 1500.00,
            "violatedRules": [
            "CryptoSanctionZone"
            ],
            "flaggedAt": "2026-06-26T14:18:02Z"
        }
    ]
    ```
