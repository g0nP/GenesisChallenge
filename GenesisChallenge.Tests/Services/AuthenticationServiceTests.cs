using GenesisChallenge.Abstractions.Repositories;
using GenesisChallenge.Abstractions.Services;
using GenesisChallenge.Core;
using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Core.Helpers.Hashing;
using GenesisChallenge.Core.Responses;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static GenesisChallenge.Core.CustomExceptions;

namespace GenesisChallenge.Tests.Services
{
    public class AuthenticationServiceTests
    {
        public class Tests : Test
        {
            [SetUp]
            public void Setup()
            {
                _config = new ConfigurationBuilder()
                .AddJsonFile("jwt.json")
                .Build();

                _userRepository = new Mock<IUserRepository>();
                _repositoryWrapper = new Mock<IRepositoryWrapper>();
                _systemClock = new Mock<ISystemClock>();
                _systemClock.Setup(c => c.GetCurrentTime()).Returns(DateTime.UtcNow);

            }

            [Test]
            [TestCase("fred", "fred@email.com", "password", Description = "Correct arguments with telephones list")]
            public void ShouldCreateUserWhenSignUpWithTelephones(string name, string email, string password)
            {
                GivenSignUpDto(name, email, password, _telephones);
                GivenUserDoesntExist();
                GivenAuthenticationService();
                WhenSignUp();
                ThenUserIsCreated();
            }

            [Test]
            [TestCase("fred", "fred@email.com", "password", Description = "Correct arguments with empty telephones list")]
            public void ShouldCreateUserWhenSignUpWithEmptyTelephones(string name, string email, string password)
            {
                GivenSignUpDto(name, email, password, _telephonesEmpty);
                GivenUserDoesntExist();
                GivenAuthenticationService();
                WhenSignUp();
                ThenUserIsCreated();
            }

            [Test]
            [TestCase("fred", "fred@email.com", "password", Description = "Correct arguments with null telephones list")]
            public void ShouldCreateUserWhenSignUpWithNullTelephones(string name, string email, string password)
            {
                GivenSignUpDto(name, email, password, _telephonesNull);
                GivenUserDoesntExist();
                GivenAuthenticationService();
                WhenSignUp();
                ThenUserIsCreated();
            }

            [Test]
            [TestCase("", "fred@email.com", "password", Description = "Empty Name")]
            [TestCase(null, "fred@email.com", "password", Description = "Null Name")]
            [TestCase("fred", "", "password", Description = "Empty Email")]
            [TestCase("fred", null, "password", Description = "Null Email")]
            [TestCase("fred", "fred@email.com", "", Description = "Empty Password")]
            [TestCase("fred", "fred@email.com", null, Description = "Null Password")]
            public void ShouldThrowArgumentNullExceptionWhenMissingSignUpArgument(string name, string email, string password)
            {
                GivenSignUpDto(name, email, password, _telephones);
                GivenAuthenticationService();
                ThenExceptionIsThrown<ArgumentNullException>(WhenSignUp);
            }

            [Test]
            [TestCase("fred", "fred@email.com", "password", "E-mail already exists")]
            public void ShouldThrowEmailAlreadyExistsExceptionDuringSignUpWhenEmailAlreadyExists(string name, string email, string password, string exceptionMessage)
            {
                GivenSignUpDto(name, email, password, _telephones);
                GivenUserExists(email, password);
                GivenAuthenticationService();
                ThenExceptionIsThrown<EmailAlreadyExistsException>(WhenSignUp, exceptionMessage);
            }

            [Test]
            [TestCase("fred@email.com", "password")]
            public void ShouldReturnRegisteredUserWhenSignIn(string email, string password)
            {
                GivenSignInDto(email, password);
                GivenUserExists(email, password);
                GivenAuthenticationService();
                WhenSignIn();
                ThenRegisteredUserIsReturned();
            }

            [Test]
            [TestCase("", "password", Description = "Empty Email")]
            [TestCase(null, "password", Description = "Null Email")]
            [TestCase("fred@email.com", "", Description = "Empty Password")]
            [TestCase("fred@email.com", null, Description = "Null Password")]
            public void ShouldThrowArgumentNullExceptionWhenMissingSignInArgument(string email, string password)
            {
                GivenSignInDto(email, password);
                GivenAuthenticationService();
                ThenExceptionIsThrown<ArgumentNullException>(WhenSignIn);
            }

            [Test]
            [TestCase("fred@email.com", "password", "Invalid user and / or password")]
            public void ShouldThrowUnexistentEmailExceptionWhenEmailDoesntExists(string email, string password, string exceptionMessage)
            {
                GivenSignInDto(email, password);
                GivenUserDoesntExist();
                GivenAuthenticationService();
                ThenExceptionIsThrown<UnexistentEmailException>(WhenSignIn, exceptionMessage);
            }

            [Test]
            [TestCase("fred@email.com", "password", "Invalid user and / or password")]
            public void ShouldThrowInvalidPasswordExceptionWhenIncorrectPassword(string email, string password, string exceptionMessage)
            {
                GivenSignInDto(email, password);
                GivenUserExists(email, "zzz");
                GivenAuthenticationService();
                ThenExceptionIsThrown<InvalidPasswordException>(WhenSignIn, exceptionMessage);
            }

            private void GivenSignUpDto(string name, string email, string password, IReadOnlyCollection<TelephoneDto> telephones)
            {
                _signUpDto = new SignUpDto
                {
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

            private void GivenUserDoesntExist()
            {
                _userRepository.Setup(p => p.FindByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(_emptyDatabase);
                _repositoryWrapper.Setup(p => p.User).Returns(_userRepository.Object);
            }

            private void GivenUserExists(string email, string password)
            {
                var salt = Salt.Create();
                var hash = Hash.Create(password, salt);

                var _databaseWithUser = new List<User> { new User { Email = email, Password = hash, Salt = salt } }.AsQueryable();

                _userRepository.Setup(p => p.FindByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(_databaseWithUser);
                _repositoryWrapper.Setup(p => p.User).Returns(_userRepository.Object);
            }

            private void GivenAuthenticationService()
            {
                _authenticationService = new AuthenticationService(_config, _repositoryWrapper.Object, _systemClock.Object);
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

                if (_signUpDto.Telephones == null)
                {
                    Assert.IsTrue(_signUpResponse.Telephones == null);
                }
                else
                {
                    Assert.IsTrue(_signUpResponse.Telephones.Count() == _signUpDto.Telephones.Count());
                    CollectionAssert.AreEquivalent(_signUpDto.Telephones, _signUpResponse.Telephones);
                }

                Assert.That(!string.IsNullOrWhiteSpace(_signUpResponse.Token));
            }

            private void ThenRegisteredUserIsReturned()
            {
                Assert.IsNotNull(_signInResponse);
                Assert.That(!string.IsNullOrWhiteSpace(_signInResponse.Token));
            }

            private static Guid[] RegisteredUserId = new Guid[] { new Guid("0700c1be-5c95-4de2-a463-2703aa65c480") };
            private static Guid[] NotRegisteredUserId = new Guid[] { new Guid("8497c1be-5c95-4de2-a463-2703aa65e784") };

            private readonly IReadOnlyCollection<TelephoneDto> _telephones = new List<TelephoneDto> { new TelephoneDto { Number = 555 }, new TelephoneDto { Number = 456 }, new TelephoneDto { Number = 789 } };
            private readonly IReadOnlyCollection<TelephoneDto> _telephonesEmpty = new List<TelephoneDto>();
            private readonly IReadOnlyCollection<TelephoneDto> _telephonesNull = null;
            private readonly IQueryable<User> _emptyDatabase = new List<User>().AsQueryable();


            private IConfigurationRoot _config;
            private Mock<ISystemClock> _systemClock;
            private Mock<IUserRepository> _userRepository;
            private Mock<IRepositoryWrapper> _repositoryWrapper;
            private IAuthenticationService _authenticationService;
            private SignUpDto _signUpDto;
            private ISignUpResponse _signUpResponse;
            private SignInDto _signInDto;
            private ISignInResponse _signInResponse;
        }
    }
}
