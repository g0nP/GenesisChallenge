using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Domain.Models;
using System.Threading.Tasks;

namespace GenesisChallenge.Core.Mappers
{
    /// <summary>
    /// Allows mapping between the classes User and UserDto
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Maps a User to a UserDto
        /// </summary>
        /// <param name="user">User to be mapped</param>
        public static async Task<UserDto> MapToUserDto(User user)
        {
            return await Task.FromResult(new UserDto {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Telephones = await TelephoneMapper.MapToTelephoneDtoAsync(user.Telephones)
            });
        }
    }
}
