using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Domain.Models
{
    public interface IUser
    {
        DateTime CreationOn { get; set; }
        string Email { get; set; }
        Guid Id { get; set; }
        DateTime LastLoginOn { get; set; }
        DateTime LastUpdatedOn { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        IEnumerable<int> Telephones { get; set; }
    }

    public class User : IUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<int> Telephones { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public DateTime LastLoginOn { get; set; }
    }
}
