using System;
using System.Linq;
using System.Linq.Expressions;

namespace GenesisChallenge.Domain.Repositories
{
    /// <summary>
    /// Basic data access operartions for the app entities
    /// </summary>
    /// <remarks>
    /// Find a user by a condition, create a new user, update an existing user
    /// </remarks>
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
    }
}
