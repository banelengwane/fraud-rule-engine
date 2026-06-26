// FraudEngine.Api/Services/FraudRuleEngineService.cs
using System.Reflection;
using FraudEngine.Api.Models;

namespace FraudEngine.Api.Services;

public interface IFraudRuleEngineService
{
    EvaluationResult Evaluate(Transaction transaction, IEnumerable<FraudRuleConfig> rules);
}

public class FraudRuleEngineService : IFraudRuleEngineService
{
    public EvaluationResult Evaluate(Transaction transaction, IEnumerable<FraudRuleConfig> rules)
    {
        var triggered = new List<string>();

        foreach (var rule in rules)
        {
            // Use reflection to inspect properties creatively and dynamically
            PropertyInfo? prop = typeof(Transaction).GetProperty(rule.Property);
            if (prop == null) continue;

            var actualValue = prop.GetValue(transaction);
            if (actualValue == null) continue;

            if (ExecuteEvaluation(actualValue, rule.Operator, rule.Value))
            {
                triggered.Add(rule.RuleName);
            }
        }

        return new EvaluationResult { TriggeredRules = triggered };
    }

    private bool ExecuteEvaluation(object actualValue, string op, string ruleValue)
    {
        return op switch
        {
            "GreaterThan" when actualValue is decimal d && decimal.TryParse(ruleValue, out var targetD) => d > targetD,
            "Equals" when actualValue is string s => s.Equals(ruleValue, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}