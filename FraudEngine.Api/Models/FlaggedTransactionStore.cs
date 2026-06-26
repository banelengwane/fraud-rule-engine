using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudEngine.Api.Models
{
    public interface IFlaggedTransactionStore
    {
        void Save(FlaggedTransactionAlert alert);
        IEnumerable<FlaggedTransactionAlert> GetAll();
    }
    public class FlaggedTransactionStore : IFlaggedTransactionStore
    {
        // thread-safe collection for highly concurrent transaction flows
        private readonly ConcurrentBag<FlaggedTransactionAlert> _storage = new();

        public void Save(FlaggedTransactionAlert alert) => _storage.Add(alert);

        public IEnumerable<FlaggedTransactionAlert> GetAll() => _storage.ToList();
        
    }
}