using System;
using System.Linq;
using System.Linq.Expressions;

namespace GenesisChallenge.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
    }
}
