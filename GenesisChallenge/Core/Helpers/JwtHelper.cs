using GenesisChallenge.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GenesisChallenge.Core.Helpers
{
    /// <summary>
    /// Encapsulates methods related to Json Web Token
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Generates a new JWT
        /// </summary>
        /// <param name="config">Source to obtain JWT configuration (Key and Issuer)</param>
        /// <param name="userInfo">User information to generate claims to add to the token</param>
        public static async Task<string> GenerateJSONWebTokenAsync(IConfiguration config, IUser userInfo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Iat, userInfo.LastLoginOn.ToShortTimeString())
            };

            var securityKey = await Task.FromResult(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])));
            var credentials = await Task.FromResult(new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            var token = await Task.FromResult(new JwtSecurityToken(
              issuer: config["Jwt:Issuer"],
              claims: claims,
              expires: userInfo.LastLoginOn.AddMinutes(30),
              signingCredentials: credentials));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
