using System;
using System.IO.Compression;

namespace SA_Management.DataAccessLayer.Infrastructure.IRepository;

public interface IUnitOfWork
{
    public ITradeRepository TradeRepository {get;}
    Task SaveChanges();

}
