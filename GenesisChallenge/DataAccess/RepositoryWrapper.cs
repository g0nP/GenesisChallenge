﻿using GenesisChallenge.Abstractions.Repositories;
using System.Threading.Tasks;

namespace GenesisChallenge.DataAccess
{
    /// <summary>
    /// Implements IRepositoryWrapper
    /// </summary>
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IUserRepository _user;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public async Task Save()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}