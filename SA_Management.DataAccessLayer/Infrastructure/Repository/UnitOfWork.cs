using System;
using SA_Management.DataAccessLayer.Data;
using SA_Management.DataAccessLayer.Infrastructure.IRepository;
using SA_Management.Models;

namespace SA_Management.DataAccessLayer.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ITradeRepository TradeRepository {get; private set;}
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        this.TradeRepository = new TradeRepository(_context);
    }


    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}