using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
        public static async Task<string> CreateAsync()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return await Task.FromResult(Convert.ToBase64String(randomBytes));
            }
        }
    }
}
