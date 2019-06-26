using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        string Token { get; set; }
        IEnumerable<Telephone> Telephones { get; set; }
    }

    public class User : IUser
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
        public IEnumerable<Telephone> Telephones { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public DateTime LastLoginOn { get; set; }
    }
}
