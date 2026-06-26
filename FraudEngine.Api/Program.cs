using System.Text.Json;
using FraudEngine.Api.Models;
using FraudEngine.Api.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFraudRuleEngineService, FraudRuleEngineService>();
builder.Services.AddSingleton<IFlaggedTransactionStore, FlaggedTransactionStore>();

var app = builder.Build();

// mock standard dynamic json config rules
const string embeddedRulesConfig = """
[
    {
        "RuleName": "MaxAmountLimit", 
        "Property": "Amount", 
        "Operator":"GreaterThan", 
        "Value": "50000"
    },
    {
        "RuleName": "CryptoSanctionZone", 
        "Property": "MerchantType", 
        "Operator":"Equals", 
        "Value":"CryptoExchange"
    }
]
""";

// endPoint 1: Process incoming transaction events
app.MapPost("/api/transactions", (
    [FromBody] Transaction tx, 
    IFraudRuleEngineService engine,
    IFlaggedTransactionStore store) =>
{
    var activeRules = JsonSerializer.Deserialize<List<FraudRuleConfig>>(embeddedRulesConfig);
    
    var assessment = engine.Evaluate(tx, activeRules);

    if (assessment.IsFraudulent)
    {
        var alert = new FlaggedTransactionAlert(
            tx.Id,
            tx.Amount,
            assessment.TriggeredRules,
            DateTime.UtcNow
        );
        store.Save(alert);

        return Results.Ok(new { status = "FLAGGED", reason = assessment.TriggeredRules });

    }

    return Results.Ok(new {status = "APPROVED" });
});

// get all flagged incidents for operations review
app.MapGet("/api/fraud/flagged", (IFlaggedTransactionStore store) =>
{
    return Results.Ok(store.GetAll());
});

app.Run();