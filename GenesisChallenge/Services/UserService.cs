using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenesisChallenge.Services
{
    public class UserService : IUserService
    {
        private IList<User> _users = new List<User>
        {
            //new User { Id = new Guid("0700c1be-5c95-4de2-a463-2703aa65c480"), Name = "Fred", Password = "123", Email = "fred@gmail.com", Telephones = new List<int> { 122, 333, 44 } },
            //new User { Id = new Guid("12cc5b02-9354-45b4-83fc-7c24996b59a4"), Name = "Alice", Password = "321", Email = "alice@gmail.com", Telephones = new List<int>()},
            //new User { Id = new Guid("08847a1e-50bb-4be6-ad0f-dba99dc9c637"), Name = "George", Password = "123", Email = "george@gmail.com", Telephones = new List<int> { 122 }},
        };


        public IUser GetUser(Guid userId)
        {
            var user = _users.Where(x => x.Id == userId).SingleOrDefault();

            if (user == null)
            {
                throw new KeyNotFoundException("User doesn't exists");
            }

            return user;
        }
    }
}
