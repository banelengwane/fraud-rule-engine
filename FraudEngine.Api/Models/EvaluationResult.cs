namespace FraudEngine.Api.Models
{
    public record EvaluationResult
    {
        public bool IsFraudulent => TriggeredRules.Any();
        public List<string> TriggeredRules { get; init; } = new();
    }
}