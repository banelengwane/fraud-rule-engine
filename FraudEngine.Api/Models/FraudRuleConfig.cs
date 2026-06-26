namespace FraudEngine.Api.Models
{
    public class FraudRuleConfig
    {
        public string RuleName { get; init; } = string.Empty;
        public string Property { get; init; } = string.Empty; // e.g Country, Amount
        public string Operator { get; init; } = string.Empty; // eg GreaterThan, Equals 
        public string Value { get; init; } = string.Empty; // the target threshold value 
    }
}