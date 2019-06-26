using GenesisChallenge.Domain.Models;
using GenesisChallenge.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenesisChallenge.Mappers
{
    public static class TelephonesMapper
    {
        public static IReadOnlyCollection<TelephoneDto> MapToTelephoneDto(IReadOnlyCollection<Telephone> telephones)
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
            return _telephonesDto;
        }

        public static IReadOnlyCollection<Telephone> MapToTelephone(IReadOnlyCollection<TelephoneDto> telephonesDto)
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
            return _telephones;
        }
    }
}
