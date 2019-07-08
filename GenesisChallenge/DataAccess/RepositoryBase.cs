using GenesisChallenge.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenesisChallenge.DataAccess
{
    /// <summary>
    /// Basic methods for data access
    /// </summary>
    /// <remarks>
    /// Provides functionality to find and entity by a condition, create a new entity and update an existing entity
    /// </remarks>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        /// <summary>
        /// Finds and entity by a given condition
        /// </summary>
        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression, bool includeRelationships = false)
        {
            var query = this.RepositoryContext.Set<T>().Where(expression);

            if (includeRelationships)
            {
                foreach (var property in this.RepositoryContext.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            
            return await query.SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }
    }
}
