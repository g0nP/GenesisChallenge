using GenesisChallenge.DataAccess;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;

namespace GenesisChallenge.Persistence
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
