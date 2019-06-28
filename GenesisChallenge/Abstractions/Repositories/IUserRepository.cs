using GenesisChallenge.Domain.Models;

namespace GenesisChallenge.Abstractions.Repositories
{
    /// <summary>
    /// Interface for the User repository
    /// </summary>
    /// <remarks>
    /// Encapsulates the data access operation for a User
    /// </remarks>
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
