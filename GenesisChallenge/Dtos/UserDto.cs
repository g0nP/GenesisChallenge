using System;
using System.Collections.Generic;

namespace GenesisChallenge.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IReadOnlyCollection<TelephoneDto> Telephones { get; set; }
    }
}
