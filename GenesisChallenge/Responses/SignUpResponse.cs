using GenesisChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Responses
{
    public interface ISignUpResponse : IUser
    {
        string Token { get; set; }
    }

    public class SignUpResponse : ISignUpResponse
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<int> Telephones { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastLoginOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        
       
    }
}
