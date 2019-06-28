using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Helpers;
using GenesisChallenge.Helpers.Hashing;
using GenesisChallenge.Infrastructure;
using GenesisChallenge.Mappers;
using GenesisChallenge.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using static GenesisChallenge.Domain.CustomExceptions;

namespace GenesisChallenge.Services
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

        public ISignInResponse SignIn(SignInDto signInDto)
        {

            var user = ValidateSignIn(signInDto);

            user.LastLoginOn = _systemClock.GetCurrentTime();
            user.LastUpdatedOn = _systemClock.GetCurrentTime();

            var token = JwtHelper.GenerateJSONWebToken(_config, user);
            user.Token = Hash.Create(token, user.Salt);

            _repository.User.Update(user);
            _repository.Save();

            var response = new SignInResponse
            {
                Id = user.Id,
                Token = token,
                CreationOn = user.CreationOn,
                LastLoginOn = user.LastLoginOn,
                LastUpdatedOn = user.LastUpdatedOn
            };

            return response;
        }

        public ISignUpResponse SignUp(SignUpDto signUpDto)
        {
            ValidateSignUp(signUpDto);

            var salt = Salt.Create();

            var user = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Salt = salt,
                Password = Hash.Create(signUpDto.Password, salt),
                Telephones = TelephoneMapper.MapToTelephone(signUpDto.Telephones),
                LastLoginOn = _systemClock.GetCurrentTime(),
                LastUpdatedOn = _systemClock.GetCurrentTime(),
                CreationOn = _systemClock.GetCurrentTime(),
            };

            var token = JwtHelper.GenerateJSONWebToken(_config, user);
            user.Token = Hash.Create(token, salt);

            _repository.User.Create(user);
            _repository.Save();

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

            return response;
        }

        private User ValidateSignIn(SignInDto signInDto)
        {
            if (string.IsNullOrWhiteSpace(signInDto.Email))
            {
                throw new ArgumentNullException(nameof(signInDto.Email));
            }

            if (string.IsNullOrWhiteSpace(signInDto.Password))
            {
                throw new ArgumentNullException(nameof(signInDto.Password));
            }
            var user = _repository.User.FindByCondition(u => string.Equals(u.Email, signInDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (user == null)
            {
                throw new UnexistentEmailException("Invalid user and / or password");
            }
            else if (!Hash.Validate(signInDto.Password, user.Salt, user.Password))
            {
                throw new InvalidPasswordException("Invalid user and / or password");
            }

            return user;
        }

        private void ValidateSignUp(SignUpDto signUpDto)
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

            var user = _repository.User.FindByCondition(u => string.Equals(u.Email, signUpDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (user != null)
            {
                throw new EmailAlreadyExistsException("E-mail already exists");
            }
        }
    }
}
