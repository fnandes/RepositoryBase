using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dlp.Buy4.RepositoryBase
{
    /// <summary>
    /// Base repository for data access with CRUD operations.
    /// </summary>
    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>Enumerable of elements with result.</returns>
        IList<T> GetAll();

        /// <summary>
        /// Async gets all items.
        /// </summary>
        /// <returns>Enumerable of elements with result.</returns>
        Task<IList<T>> GetAllAsync();

        /// <summary>
        /// Gets a item by their key.
        /// </summary>
        /// <param name="keyValues">values thats corresponds to object key.</param>
        T GetById(params object[] keyValues);

        /// <summary>
        /// Async gets a item by their key.
        /// </summary>
        /// <param name="keyValues">values thats corresponds to object key.</param>
        Task<T> GetByIdAsync(params object[] keyValues);

        /// <summary>
        /// Adds a new object into data store.
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Newly added entity.</returns>
        T Add(T entity);

        /// <summary>
        /// Async adds a new object into data store.
        /// </summary>
        /// <param name="entity">Entity to be created</param>
        /// <returns>Newly added entity.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Removes an entity from data store.
        /// </summary>
        /// <param name="entity">Entity to be removed.</param>
        void Remove(T entity);

        /// <summary>
        /// Async removes an entity from data store.
        /// </summary>
        /// <param name="entity">Entity to be removed.</param>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entity data to be updated.</param>
        void Update(T entity);

        /// <summary>
        /// Async updates an entity.
        /// </summary>
        /// <param name="entity">Entity data to be updated.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Performs a bulkcopy (SqlServer) with the collection of entities.
        /// </summary>
        /// <param name="entityCollection">Collection to be bulk inserted.</param>
        void BulkInsert(ICollection<T> entityCollection);
    }
}
