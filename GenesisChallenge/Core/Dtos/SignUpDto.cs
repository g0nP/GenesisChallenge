using System.Collections.Generic;

namespace GenesisChallenge.Core.Dtos
{
    public class SignUpDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IReadOnlyCollection<TelephoneDto> Telephones { get; set; }
    }
}
