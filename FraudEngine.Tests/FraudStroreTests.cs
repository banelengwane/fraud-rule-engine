using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FraudEngine.Api.Models;
using FraudEngine.Api.Services;

namespace FraudEngine.Tests
{
    public class FraudStroreTests
    {
        [Fact]
        public void Store_ShouldSaveAndRetrieveFlaggedTrnsactions()
        {
            // Arrange
            var store = new FlaggedTransactionStore();
            var alert = new FlaggedTransactionAlert(
                "tx-999",
                1200.50m,
                new List<string> { "HighRiskMerchant" },
                DateTime.UtcNow
            );
        
            // Act 
            store.Save(alert);
            var items = store.GetAll();
        
            // Assert
            Assert.Single(items);
            Assert.Equal("tx-999", items.First().TransactionId);
        }
    }
}