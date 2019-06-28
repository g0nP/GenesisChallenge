using GenesisChallenge.DataAccess;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;

namespace GenesisChallenge.Persistence
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
