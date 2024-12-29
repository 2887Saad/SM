using System;

namespace SA_Management.Models;

public class CompositeKey
{
    public BrokerDetails BrokerDetails { get; set; }
    public Guid BrokerID{get;set;}
    public TradeDetails TradeDetails { get; set; }
    public Guid TradeDetailsID{get;set;}
}
