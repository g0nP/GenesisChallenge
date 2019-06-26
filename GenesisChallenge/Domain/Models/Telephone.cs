using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Domain.Models
{
    public class Telephone
    {
        public int TelephoneId { get; set; }
        public int Number { get; set; }
    }
}
