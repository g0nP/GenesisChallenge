using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenesisChallenge.Core.Mappers
{
    /// <summary>
    /// Allows mapping between the classes Telephone and TelephoneDto
    /// </summary>
    public static class TelephoneMapper
    {
        /// <summary>
        /// Maps a collection of Telephone to a collection of TelephoneDto
        /// </summary>
        /// <param name="telephones">Collection of Telephone to be mapped</param>
        public static async Task<IReadOnlyCollection<TelephoneDto>> MapToTelephoneDtoAsync(IReadOnlyCollection<Telephone> telephones)
        {
            IReadOnlyCollection<TelephoneDto> _telephonesDto = null;

            if (telephones != null)
            {
                var aux = new List<TelephoneDto>();

                foreach (var telephone in telephones)
                {
                    aux.Add(new TelephoneDto { Number = telephone.Number });
                }

                _telephonesDto = aux;
            }
            return await Task.FromResult(_telephonesDto);
        }

        /// <summary>
        /// Maps a collection of TelephoneDto to a collection of Telephone
        /// </summary>
        /// <param name="telephonesDto">Collection of TelephoneDto to be mapped</param>
        public static async Task<IReadOnlyCollection<Telephone>> MapToTelephoneAsync(IReadOnlyCollection<TelephoneDto> telephonesDto)
        {
            IReadOnlyCollection<Telephone> _telephones = null;


            if (telephonesDto != null)
            {
                var aux = new List<Telephone>();

                foreach (var telephone in telephonesDto)
                {
                    aux.Add(new Telephone { Number = telephone.Number });
                }

                _telephones = aux;
            }
            return await Task.FromResult(_telephones);
        }
    }
}
