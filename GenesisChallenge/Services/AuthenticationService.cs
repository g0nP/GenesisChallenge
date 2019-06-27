using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Repositories;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Infrastructure;
using GenesisChallenge.Mappers;
using GenesisChallenge.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static GenesisChallenge.Domain.CustomExceptions;

namespace GenesisChallenge.Services
{
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
            ValidateSignIn(signInDto);

            var user = _repository.User.FindByCondition(u => string.Equals(u.Email, signInDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            user.LastLoginOn = _systemClock.GetCurrentTime();
            user.LastUpdatedOn = _systemClock.GetCurrentTime();
            user.Token = GenerateJSONWebToken(user);

            _repository.User.Update(user);
            _repository.Save();

            var response = new SignInResponse
            {
                Id = user.Id,
                Token = user.Token,
                CreationOn = user.CreationOn,
                LastLoginOn = user.LastLoginOn,
                LastUpdatedOn = user.LastUpdatedOn
            };

            return response;
        }

        public ISignUpResponse SignUp(SignUpDto signUpDto)
        {
            ValidateSignUp(signUpDto);

            var user = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                Telephones = TelephonesMapper.MapToTelephone(signUpDto.Telephones),
                LastLoginOn = _systemClock.GetCurrentTime(),
                LastUpdatedOn = _systemClock.GetCurrentTime(),
                CreationOn = _systemClock.GetCurrentTime(),
            };

            var token = GenerateJSONWebToken(user);
            user.Token = token;

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

        private void ValidateSignIn(SignInDto signInDto)
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
                throw new InexistentEmailException("Invalid user and / or password");
            }
            else if (user.Password != signInDto.Password)
            {
                throw new InvalidPasswordException("Invalid user and / or password");
            }
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

        private string GenerateJSONWebToken(IUser userInfo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Iat, userInfo.LastLoginOn.ToShortTimeString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              claims: claims,
              expires: userInfo.LastLoginOn.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
