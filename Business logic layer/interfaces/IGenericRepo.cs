using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.interfaces
{
    public interface IGenericRepo<T>
    {
        // Fixed syntax errors and ensured valid collection type for params
        public Task<IEnumerable<T>> GetAllAsync(
             Expression<Func<T, bool>> filter = null,
             string includeProperties = "");
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);

        void UpdateAsync(T entity);

        void DeleteAsync(T entity);
        Task<T> FindFirstAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        bool disableTracking = true);
    }

}
