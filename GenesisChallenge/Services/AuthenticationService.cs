using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static GenesisChallenge.Domain.CustomExceptions;

namespace GenesisChallenge.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IList<User> _users = new List<User>
        {
            new User { Id = new Guid("0700c1be-5c95-4de2-a463-2703aa65c480"), Name = "Fred", Password = "123", Email = "fred@gmail.com", Telephones = new List<int> { 122, 333, 44 } },
            new User { Id = new Guid("12cc5b02-9354-45b4-83fc-7c24996b59a4"), Name = "Alice", Password = "321", Email = "alice@gmail.com", Telephones = new List<int>()},
            new User { Id = new Guid("08847a1e-50bb-4be6-ad0f-dba99dc9c637"), Name = "George", Password = "123", Email = "george@gmail.com", Telephones = new List<int> { 122 }},
        };

        private IConfiguration _config;

        public AuthenticationService(IConfiguration config)
        {
            _config = config;
        }

        public ISignInResponse SignIn(SignInDto signInDto)
        {
            ValidateSignIn(signInDto);

            var user = _users.Where(u => string.Equals(u.Email, signInDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            user.LastLoginOn = DateTime.UtcNow;
            //TODO: save login date

            var token = GenerateJSONWebToken(user);

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

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                Telephones = signUpDto.Telephones,
                CreationOn = DateTime.UtcNow,
                LastLoginOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow
            };

            _users.Add(user);

            var token = GenerateJSONWebToken(user);
            var response = new SignUpResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Telephones = user.Telephones,
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

            var user = _users.Where(u => string.Equals(u.Email, signInDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

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

            var user = _users.Where(u => string.Equals(u.Email, signUpDto.Email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (user != null)
            {
                throw new EmailAlreadyExistsException("E-mail already exists");
            }
        }

        private string GenerateJSONWebToken(IUser userInfo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
