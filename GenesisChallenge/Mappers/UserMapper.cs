using GenesisChallenge.Domain.Models;
using GenesisChallenge.Dtos;
using System.Collections.Generic;

namespace GenesisChallenge.Mappers
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
        public static UserDto MapToUserDto(User user)
        {
            return new UserDto {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Telephones = TelephoneMapper.MapToTelephoneDto(user.Telephones)
            };
        }
    }
}
