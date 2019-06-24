using GenesisChallenge.Domain.Models;
using System;

namespace GenesisChallenge.Domain.Services
{
    public interface IUserService
    {
        IUser GetUser(Guid userId);
    }
}
