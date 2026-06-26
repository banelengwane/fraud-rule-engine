namespace FraudEngine.Api.Models
{
    public record Transaction
    {
        public string Id { get; init; } = string.Empty;
        public string AccountNumber { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string Country { get; init; } = string.Empty;
        public string MerchantType { get; init; } = string.Empty;
    }
}