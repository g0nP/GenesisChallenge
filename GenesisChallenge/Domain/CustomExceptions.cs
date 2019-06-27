using System;

namespace GenesisChallenge.Domain
{
    /// <summary>
    /// Custom exceptions related to business logic
    /// </summary>
    public class CustomExceptions
    {
        /// <summary>
        /// Exception for when an email already exists in the database
        /// </summary>
        /// <remarks>
        /// Should be thrown during the creation of a new user when there already exists another user with the same email
        /// </remarks>
        [Serializable]
        public class EmailAlreadyExistsException : Exception
        {
            public EmailAlreadyExistsException() { }

            public EmailAlreadyExistsException(string message)
                : base(message)
            { }
        }

        /// <summary>
        /// Exception for when an email doesnt exists in the database
        /// </summary>
        /// <remarks>
        /// Should be thrown during the sign in when there are no users with the provided email credential
        /// </remarks>
        [Serializable]
        public class InexistentEmailException : Exception
        {
            public InexistentEmailException() { }

            public InexistentEmailException(string message)
                : base(message)
            { }
        }

        /// <summary>
        /// Exception for when the user's provided password in invalid
        /// </summary>
        /// <remarks>
        /// Should be thrown during sing in when the password provided by the user doesn´t match the one registered in the database.
        /// </remarks>
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
