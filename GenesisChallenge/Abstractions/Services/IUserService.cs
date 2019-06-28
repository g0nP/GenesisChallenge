using GenesisChallenge.Core.Dtos;
using System;

namespace GenesisChallenge.Abstractions.Services
{
    /// <summary>
    /// Service for operations concerning users
    /// </summary>
    /// <remarks>
    /// Allows searching for a user
    /// </remarks>
    public interface IUserService
    {
        /// <summary>
        /// Search for a user
        /// </summary>
        /// <param name="userId">Id of the user to be searched out</param>
        /// <param name="accessToken">JWT issued to the user performing the search</param>
        UserDto GetUser(Guid userId, string accessToken);
    }
}
