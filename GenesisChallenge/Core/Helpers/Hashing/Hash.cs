using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GenesisChallenge.Core.Helpers.Hashing
{
    /// <summary>
    /// Encapsulates methods related to hashing
    /// </summary>
    public class Hash
    {
        /// <summary>
        /// Generates a new hash
        /// </summary>
        /// <param name="value">Value to be hashed</param>
        /// <param name="salt">Key to apply during the hashing process</param>
        public static async Task<string> CreateAsync(string value, string salt)
        {
            var valueBytes = await Task.FromResult(KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));

            return await Task.FromResult(Convert.ToBase64String(valueBytes));
        }

        /// <summary>
        /// Checks if a given value matches a given hash
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="salt">Key applied during the generation of the given hash</param>
        /// <param name="hash">Hash to validate against</param>
        public static async Task<bool> Validate(string value, string salt, string hash)
            => await CreateAsync(value, salt) == hash;
    }
}
