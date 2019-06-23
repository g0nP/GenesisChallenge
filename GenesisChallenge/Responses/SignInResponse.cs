using GenesisChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Responses
{
    public interface ISignInResponse
    {
        DateTime CreationOn { get; set; }
        Guid Id { get; set; }
        DateTime LastLoginOn { get; set; }
        DateTime LastUpdatedOn { get; set; }
        string Token { get; set; }
    }

    public class SignInResponse : ISignInResponse
    {
        public Guid Id { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public DateTime LastLoginOn { get; set; }
        public string Token { get; set; }
    }
}
