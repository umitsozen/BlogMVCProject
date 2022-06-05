using Blog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Data.Abstract
{
    /// <summary>
    /// Generic Repository
    /// DAL class'larımızdaki ortak metotlar yer almaktadır. Metotlar Aysnc veya Sync olabilir.
    /// GET(),GETALL(), ADD(), UPDATE(), DELETE(), ANY(), COUNT()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityRepository<T> where T : class,IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T,bool>> preditcate,params Expression<Func<T, object>>[] includeProperties);

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> preditcate=null, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);    
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
