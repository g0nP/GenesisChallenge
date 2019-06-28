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

        public UserDto GetUser(Guid userId, string accessToken)
        {
            var user = _repository.User.FindByCondition(x => x.Id == userId, true).SingleOrDefault();

            ValidateUser(user, accessToken);

            return UserMapper.MapToUserDto(user);
        }

        private void ValidateUser(IUser user, string token)
        {
            if (user == null)
            {
                throw new KeyNotFoundException("User doesn't exists");
            }

            if (token == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (!Hash.Validate(token, user.Salt, user.Token))
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
