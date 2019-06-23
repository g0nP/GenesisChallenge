using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Domain
{
    public class CustomExceptions
    {
        [Serializable]
        public class EmailAlreadyExistsException : Exception
        {
            public EmailAlreadyExistsException() { }

            public EmailAlreadyExistsException(string message)
                : base(message)
            { }
        }

        [Serializable]
        public class InexistentEmailException : Exception
        {
            public InexistentEmailException() { }

            public InexistentEmailException(string message)
                : base(message)
            { }
        }

        [Serializable]
        public class InvalidPasswordException : Exception
        {
            public InvalidPasswordException() { }

            public InvalidPasswordException(string message)
                : base(message)
            { }
        }
    }
}
