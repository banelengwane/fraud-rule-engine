using System.Text.Json;
using FraudEngine.Api.Models;
using FraudEngine.Api.Services;

namespace FraudEngine.Tests
{
    public class RuleEngineTest
    {
        [Fact]
        public void Evaluate_ShouldFlagTransaction_WhenAmountExceedsThreshold()
        {
            // Arrane
            var engine = new FraudRuleEngineService();

            var transaction = new Transaction
            {
                Id = "tx-101",
                AccountNumber = "ACC-9988",
                Amount = 15000.00m,
                Country = "ZA",
                MerchantType = "CryptoExchange"
            };

            // Simulating a JSON config file loaded at runtime
            string configJson = @"
            [
                {
                    ""RuleName"": ""HighValueTransaction"",
                    ""Property"": ""Amount"",
                    ""Operator"": ""GreaterThan"",
                    ""Value"": ""10000""
                }
            ]";

            var rules = JsonSerializer.Deserialize<List<FraudRuleConfig>>(configJson);
        
            // Act 
            var result = engine.Evaluate(transaction, rules!);

            // assert 
            Assert.True(result.IsFraudulent);
            Assert.Contains("HighValueTransaction", result.TriggeredRules);
        }

        [Fact]
        public void Evaluate_ShouldFlagTransction_WhenCountryIsRestricted()
        {
            // Arrange 
            var engine = new FraudRuleEngineService();
            var transaction = new Transaction { Id = "tx-102", Country = "SuspiciousCountry"};

            var rules = new List<FraudRuleConfig>
            {
                new() { RuleName = "RestrictedRegion", Property = "Country", Operator = "Equals", Value = "SuspiciousCountry"}
            };
        
            // Act 
            var result = engine.Evaluate(transaction, rules);
        
            // Assert 
            Assert.True(result.IsFraudulent);
            Assert.Contains("RestrictedRegion", result.TriggeredRules);
        }
    }
}