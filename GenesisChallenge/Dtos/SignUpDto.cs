using GenesisChallenge.Domain.Models;
using System.Collections.Generic;

namespace GenesisChallenge.Dtos
{
    public class SignUpDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<TelephoneDto> Telephones { get; set; }
    }
}
