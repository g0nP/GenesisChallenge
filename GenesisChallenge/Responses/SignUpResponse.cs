using GenesisChallenge.Domain.Models;
using GenesisChallenge.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Responses
{
    public interface ISignUpResponse
    {
        DateTime CreationOn { get; set; }
        string Email { get; set; }
        Guid Id { get; set; }
        DateTime LastLoginOn { get; set; }
        DateTime LastUpdatedOn { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string Token { get; set; }
        IReadOnlyCollection<TelephoneDto> Telephones { get; set; }
    }

    public class SignUpResponse : ISignUpResponse
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IReadOnlyCollection<TelephoneDto> Telephones { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastLoginOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
