using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;

namespace SA_Management.Services;

public class QueryService
{
    private readonly AppDbContext _context;

    public QueryService(AppDbContext context)
    {
        _context = context;
    }

    public void GetTrade()
    {
        var data = _context.Trades.GroupBy(t=>t.TradeType).Select(group=>new{
            Name = group.Key,
            Total = group.Count(),
            Sales = group.Sum(t=>t.Quantity)
        });
        Console.WriteLine(data);
    }

}
