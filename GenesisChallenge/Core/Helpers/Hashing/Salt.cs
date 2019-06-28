using System;
using System.Security.Cryptography;

namespace GenesisChallenge.Core.Helpers.Hashing
{
    /// <summary>
    /// Encapsulates methods related to hashing keys
    /// </summary>
    public class Salt
    {
        /// <summary>
        /// Creates a key to be used during the hashing process
        /// </summary>
        public static string Create()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
