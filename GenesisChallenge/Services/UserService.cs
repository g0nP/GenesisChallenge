using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Helpers.Hashing;
using GenesisChallenge.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenesisChallenge.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repository;
        private ISystemClock _systemClock;

        public UserService(IRepositoryWrapper repository, ISystemClock systemClock)
        {
            _repository = repository;
            _systemClock = systemClock;
        }

        public IUser GetUser(Guid userId, string accessToken)
        {
            var user = _repository.User.FindByCondition(x => x.Id == userId).SingleOrDefault();

            ValidateUser(user, accessToken);

            return user;
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
