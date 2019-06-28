using GenesisChallenge.Abstractions.Repositories;
using GenesisChallenge.Domain.Models;

namespace GenesisChallenge.DataAccess
{
    /// <summary>
    /// Implements IUserRepository
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
