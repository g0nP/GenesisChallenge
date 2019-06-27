using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Helpers.Hashing;
using GenesisChallenge.Infrastructure;
using GenesisChallenge.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenesisChallenge.Services
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
            var user = _repository.User.FindByCondition(x => x.Id == userId).SingleOrDefault();

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
