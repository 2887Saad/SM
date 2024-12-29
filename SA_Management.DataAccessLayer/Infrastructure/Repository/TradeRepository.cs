using System;
using SA_Management.DataAccessLayer.Data;
using SA_Management.DataAccessLayer.Infrastructure.IRepository;
using SA_Management.Models;

namespace SA_Management.DataAccessLayer.Infrastructure.Repository;

public class TradeRepository : Repository<Trade>, ITradeRepository
{
    public TradeRepository(AppDbContext context) : base(context)
    {
        
    }
}
