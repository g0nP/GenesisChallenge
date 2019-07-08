using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenesisChallenge.Abstractions.Repositories
{
    /// <summary>
    /// Basic data access operartions for the app entities
    /// </summary>
    /// <remarks>
    /// Find a user by a condition, create a new user, update an existing user
    /// </remarks>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Finds and entity by a given condition
        /// </summary>
        /// <param name="expression">Condition to search by</param>
        /// <param name="includeRelationships">Includes or not every relationship data of the entity</param>
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression, bool includeRelationships = false);

        /// <summary>
        /// Creates a new entity
        /// </summary>
        void Create(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        void Update(T entity);
    }
}
