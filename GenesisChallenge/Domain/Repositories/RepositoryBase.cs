using GenesisChallenge.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GenesisChallenge.Domain.Repositories
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
            this.RepositoryContext = repositoryContext;
        }

        /// <summary>
        /// Finds and entity by a given condition
        /// </summary>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
    }
}
