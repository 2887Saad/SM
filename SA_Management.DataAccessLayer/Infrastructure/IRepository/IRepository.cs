using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace SA_Management.DataAccessLayer.Infrastructure.IRepository;

public interface IRepository<T> where T : class
{
   Task<IList<T>> GetAll();
    Task<T> Create(T obj);
    Task<T> Update(T obj);
    T Delete(T obj);
    Task<T> GetT(Expression<Func<T,bool>> predicate);
}