using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    /// <summary>
    /// Defines various methods for working with data in the system.
    /// </summary>
    public interface IMapper<T, K> where T : DomainEntity<K>
    {
        /// <summary>
        /// Returns an IQueryable of items of type T.
        /// </summary>
        /// <param name="predicate">A predicate to limit the items being returned.</param>
        /// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
        /// <returns>An IEnumerable of the requested type T.</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, string sort, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Returns an IQueryable of items of type T.
        /// </summary>
        /// <param name="predicate">A predicate to limit the items being returned.</param>
        /// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
        /// <returns>An IEnumerable of the requested type T.</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Returns an IQueryable of all items of type T.
        /// </summary>
        /// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
        /// <returns>An IQueryable of the requested type T.</returns>
        IQueryable<T> FindAll(PagingCriteria pagingCriteria, out int totalCount, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Returns an IQueryable of items of type T.
        /// </summary>
        /// <param name="predicate">A predicate to limit the items being returned.</param>
        /// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
        /// <returns>An IEnumerable of the requested type T.</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, out int totalCount, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Returns an IQueryable of items of type T.
        /// </summary>
        /// <param name="predicate">A predicate to limit the items being returned.</param>
        /// <param name="includeProperties">An expression of additional properties to eager load. For example: x => x.SomeCollection, x => x.SomeOtherCollection.</param>
        /// <returns>An IEnumerable of the requested type T.</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, PagingCriteria pagingCriteria, out int totalCount, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        T FindFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

       Error Save(T oEntity, bool isNew = true);
        Task<Error> SaveAsync(T oEntity, bool isNew = true);
        Error Delete(T oEntity);
        Task<Error> DeleteAsync(T oEntity);
        Task<Error> ObsoleteAsync(T oEntity, DateTime? ObsoletionDate = null);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        M Max<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector);
        Task<M> MaxAsync<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector);
        Task<M> MinAsync<M>(Expression<Func<T, bool>> predicate, Expression<Func<T, M>> selector);
    }
}
