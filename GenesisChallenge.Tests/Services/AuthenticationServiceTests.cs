using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;
using GenesisChallenge.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static GenesisChallenge.Domain.CustomExceptions;

namespace GenesisChallenge.Tests.Services
{
    public class AuthenticationServiceTests
    {
        public class Tests : Test
        {
            [SetUp]
            public void Setup()
            {
                var config = new ConfigurationBuilder()
                .AddJsonFile("jwt.json")
                .Build();

                _authenticationService = new AuthenticationService(config);
            }

            [Test]
            [TestCase("fred", "fred@email.com", "password", new int[] { 123456, 544522 }, Description = "Correct arguments with telephones list")]
            [TestCase("fred", "fred@email.com", "password", new int[] { }, Description = "Correct arguments with empty telephones list")]
            [TestCase("fred", "fred@email.com", "password", null, Description = "Correct arguments with null telephones list")]
            public void ShouldCreateUserWhenSignUp(string name, string email, string password, IEnumerable<int> telephones)
            {
                GivenSignUpDto(name, email, password, telephones);
                WhenSignUp();
                ThenUserIsCreated();
            }

            [Test]
            [TestCase("", "fred@email.com", "password", new int[] { 123456, 544522 }, Description = "Empty Name")]
            [TestCase(null, "fred@email.com", "password", new int[] { 123456, 544522 }, Description = "Null Name")]
            [TestCase("fred", "", "password", new int[] { 123456, 544522 }, Description = "Empty Email")]
            [TestCase("fred", null, "password", new int[] { 123456, 544522 }, Description = "Null Email")]
            [TestCase("fred", "fred@email.com", "", new int[] { 123456, 544522 }, Description = "Empty Password")]
            [TestCase("fred", "fred@email.com", null, new int[] { 123456, 544522 }, Description = "Null Password")]
            public void ShouldThrowArgumentNullExceptionWhenMissingSignUpArgument(string name, string email, string password, IEnumerable<int> telephones)
            {
                GivenSignUpDto(name, email, password, telephones);
                ThenExceptionIsThrown<ArgumentNullException>(WhenSignUp);
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCase("fred", "fred@email.com", "password", new int[] { 123456, 544522 })]
            public void ShouldThrowEmailAlreadyExistsExceptionWhenEmailAlreadyExists(string name, string email, string password, IEnumerable<int> telephones)
            {
                GivenSignUpDto(name, email, password, telephones);
                ThenExceptionIsThrown<EmailAlreadyExistsException>(WhenSignUp);
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCase("fred", "fred@email.com", "password")]
            public void ShouldReturnRegisteredUserWhenSignIn(string email, string password)
            {
                GivenSignInDto(email, password);
                WhenSignIn();
                ThenRegisteredUserIsReturned();
            }

            [Test]
            [TestCase("", "password", Description = "Empty Email")]
            [TestCase(null, "password",Description = "Null Email")]
            [TestCase("fred@email.com", "", Description = "Empty Password")]
            [TestCase("fred@email.com", null, Description = "Null Password")]
            public void ShouldThrowArgumentNullExceptionWhenMissingSignInArgument(string email, string password)
            {
                GivenSignInDto(email, password);
                ThenExceptionIsThrown<ArgumentNullException>(WhenSignIn);
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCase("fred", "fred@email.com", "password")]
            public void ShouldThrowInexistentEmailExceptionWhenEmailDoesntExists(string email, string password)
            {
                GivenSignInDto(email, password);
                ThenExceptionIsThrown<InexistentEmailException>(WhenSignIn);
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCase("fred", "fred@email.com", "password")]
            public void ShouldThrowInvalidPasswordExceptionWhenIncorrectPassword(string email, string password)
            {
                GivenSignInDto(email, password);
                ThenExceptionIsThrown<InvalidPasswordException>(WhenSignIn);
            }

            private void GivenSignUpDto(string name, string email, string password, IEnumerable<int> telephones)
            {
                _signUpDto = new SignUpDto {
                    Name = name,
                    Email = email,
                    Password = password,
                    Telephones = telephones
                };
            }

            private void GivenSignInDto(string email, string password)
            {
                _signInDto = new SignInDto
                {
                    Email = email,
                    Password = password
                };
            }

            private void WhenSignUp()
            {
                _signUpResponse = _authenticationService.SignUp(_signUpDto);
            }

            private void WhenSignIn()
            {
                _signInResponse = _authenticationService.SignIn(_signInDto);
            }

            private void ThenUserIsCreated()
            {
                Assert.IsNotNull(_signUpResponse);
                Assert.IsTrue(string.Equals(_signUpResponse.Name, _signUpDto.Name, StringComparison.OrdinalIgnoreCase));
                Assert.IsTrue(string.Equals(_signUpResponse.Email, _signUpDto.Email, StringComparison.OrdinalIgnoreCase));
                Assert.IsTrue(string.Equals(_signUpResponse.Password, _signUpDto.Password, StringComparison.OrdinalIgnoreCase));
                Assert.That(_signUpResponse.Telephones == _signUpDto.Telephones);
                Assert.That(!string.IsNullOrWhiteSpace(_signUpResponse.Token));
            }

            private void ThenRegisteredUserIsReturned()
            {
                Assert.IsNotNull(_signInResponse);
                Assert.That(!string.IsNullOrWhiteSpace(_signUpResponse.Token));
            }

            private static Guid[] RegisteredUserId = new Guid[] { new Guid("0700c1be-5c95-4de2-a463-2703aa65c480") };
            private static Guid[] NotRegisteredUserId = new Guid[] { new Guid("8497c1be-5c95-4de2-a463-2703aa65e784") };

            private IAuthenticationService _authenticationService;
            private SignUpDto _signUpDto;
            private ISignUpResponse _signUpResponse;
            private SignInDto _signInDto;
            private ISignInResponse _signInResponse;
        }
    }
}
