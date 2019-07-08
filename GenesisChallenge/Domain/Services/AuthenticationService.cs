using GenesisChallenge.Abstractions.Repositories;
using GenesisChallenge.Abstractions.Services;
using GenesisChallenge.Core;
using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Core.Helpers;
using GenesisChallenge.Core.Helpers.Hashing;
using GenesisChallenge.Core.Mappers;
using GenesisChallenge.Core.Responses;
using GenesisChallenge.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using static GenesisChallenge.Core.CustomExceptions;

namespace GenesisChallenge.Domain.Services
{
    /// <summary>
    /// Implements IAuthenticationService
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private IRepositoryWrapper _repository;
        private IConfiguration _config;
        private ISystemClock _systemClock;

        public AuthenticationService(IConfiguration config, IRepositoryWrapper repository, ISystemClock systemClock)
        {
            _config = config;
            _repository = repository;
            _systemClock = systemClock;
        }

        public async Task<ISignInResponse> SignIn(SignInDto signInDto)
        {

            var user = await ValidateSignIn(signInDto);

            user.LastLoginOn = _systemClock.GetCurrentTime();
            user.LastUpdatedOn = _systemClock.GetCurrentTime();

            var token = await JwtHelper.GenerateJSONWebTokenAsync(_config, user);
            user.Token = await Hash.CreateAsync(token, user.Salt);

            _repository.User.Update(user);
            await _repository.Save();

            var response = new SignInResponse
            {
                Id = user.Id,
                Token = token,
                CreationOn = user.CreationOn,
                LastLoginOn = user.LastLoginOn,
                LastUpdatedOn = user.LastUpdatedOn
            };

            return await Task.FromResult(response);
        }

        public async Task<ISignUpResponse> SignUp(SignUpDto signUpDto)
        {
            await ValidateSignUp(signUpDto);

            var salt = await Salt.CreateAsync();

            var user = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Salt = salt,
                Password = await Hash.CreateAsync(signUpDto.Password, salt),
                Telephones = await TelephoneMapper.MapToTelephoneAsync(signUpDto.Telephones),
                LastLoginOn = _systemClock.GetCurrentTime(),
                LastUpdatedOn = _systemClock.GetCurrentTime(),
                CreationOn = _systemClock.GetCurrentTime(),
            };

            var token = await JwtHelper.GenerateJSONWebTokenAsync(_config, user);
            user.Token = await Hash.CreateAsync(token, salt);

            _repository.User.Create(user);
            await _repository.Save();

            var response = new SignUpResponse
            {
                Id = user.Id,
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                Telephones = signUpDto.Telephones,
                Token = token,
                CreationOn = user.CreationOn,
                LastLoginOn = user.LastLoginOn,
                LastUpdatedOn = user.LastUpdatedOn
            };

            return await Task.FromResult(response);
        }

        private async Task<User> ValidateSignIn(SignInDto signInDto)
        {
            if (string.IsNullOrWhiteSpace(signInDto.Email))
            {
                throw new ArgumentNullException(nameof(signInDto.Email));
            }

            if (string.IsNullOrWhiteSpace(signInDto.Password))
            {
                throw new ArgumentNullException(nameof(signInDto.Password));
            }
            var user = await _repository.User.FindByConditionAsync(u => string.Equals(u.Email, signInDto.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                throw new UnexistentEmailException("Invalid user and / or password");
            }
            else if (! await Hash.Validate(signInDto.Password, user.Salt, user.Password))
            {
                throw new InvalidPasswordException("Invalid user and / or password");
            }

            return user;
        }

        private async Task ValidateSignUp(SignUpDto signUpDto)
        {
            if (string.IsNullOrWhiteSpace(signUpDto.Email))
            {
                throw new ArgumentNullException(nameof(signUpDto.Email));
            }

            if (string.IsNullOrWhiteSpace(signUpDto.Password))
            {
                throw new ArgumentNullException(nameof(signUpDto.Password));
            }

            if (string.IsNullOrWhiteSpace(signUpDto.Name))
            {
                throw new ArgumentNullException(nameof(signUpDto.Name));
            }

            var user = await _repository.User.FindByConditionAsync(u => string.Equals(u.Email, signUpDto.Email, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                throw new EmailAlreadyExistsException("E-mail already exists");
            }
        }
    }
}
