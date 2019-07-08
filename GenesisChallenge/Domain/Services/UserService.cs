using GenesisChallenge.Abstractions.Repositories;
using GenesisChallenge.Abstractions.Services;
using GenesisChallenge.Core;
using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Core.Helpers.Hashing;
using GenesisChallenge.Core.Mappers;
using GenesisChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Domain.Services
{
    /// <summary>
    /// Implements IUserService
    /// </summary>
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repository;
        private ISystemClock _systemClock;

        public UserService(IRepositoryWrapper repository, ISystemClock systemClock)
        {
            _repository = repository;
            _systemClock = systemClock;
        }

        public async Task<UserDto> GetUser(Guid userId, string accessToken)
        {
            var user = await _repository.User.FindByConditionAsync(x => x.Id == userId, true);

            await ValidateUser(user, accessToken);

            return await UserMapper.MapToUserDto(user);
        }

        private async Task ValidateUser(IUser user, string token)
        {
            if (user == null)
            {
                throw new KeyNotFoundException("User doesn't exists");
            }

            if (token == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            var isValidHash = await Hash.Validate(token, user.Salt, user.Token);

            if (!isValidHash)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (user.LastLoginOn <= _systemClock.GetCurrentTime().AddMinutes(-30))
            {
                throw new UnauthorizedAccessException("Invalid Session");
            }
        }
    }
}
