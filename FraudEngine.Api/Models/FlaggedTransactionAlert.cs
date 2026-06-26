using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FraudEngine.Api.Models;
public record FlaggedTransactionAlert(
    string TransactionId,
    decimal Amount, 
    List<string> ViolatedRules,
    DateTime FlaggedAt
);
