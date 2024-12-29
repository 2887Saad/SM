using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SA_Management.DataAccessLayer.Data;
using SA_Management.DataAccessLayer.Infrastructure.IRepository;

namespace SA_Management.DataAccessLayer.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context  = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<T> Create(T obj)
    {
        await _dbSet.AddAsync(obj);
        return obj;
    }

    public T Delete(T obj)
    {
        _dbSet.Remove(obj);
        return obj;
    }

    public async Task<IList<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetT(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<T> Update(T obj)
    {
        throw new NotImplementedException();
    }
}
