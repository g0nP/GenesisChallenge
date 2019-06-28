using GenesisChallenge.Abstractions.Repositories;
using GenesisChallenge.Abstractions.Services;
using GenesisChallenge.Core;
using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GenesisChallenge.Tests.Services
{
    public class UserServiceTests
    {
        public class Tests : Test
        {
            [SetUp]
            public void Setup()
            {
                _userRepository = new Mock<IUserRepository>();
                _repositoryWrapper = new Mock<IRepositoryWrapper>();
                _systemClock = new Mock<ISystemClock>();
            }

            [Test]
            public void ShouldReturnUserWhenGetUser()
            {
                GivenUserId(SomeUserId);
                GivenToken(SomeToken);
                GivenUserInDatabase(SomeUserId, SomeToken, SomeLogInDateTime);
                GivenSystemClock(_aDateTimeLessThan30MinAfterUserLogIn);
                GivenUserService();
                WhenGetUser();
                ThenUserIsFound();
            }

            [Test]
            public void ShouldThrowKeyNotFoundExceptionWhenUserDoesntExists()
            {
                GivenUserId(SomeUserId);
                GivenToken(SomeToken);
                GivenNoUserInDatabase();
                GivenUserService();
                ThenExceptionIsThrown<KeyNotFoundException>(WhenGetUser, "User doesn't exists");
            }

            [Test]
            public void ShouldThrowUnauthorizedAccessExceptionWhenMissingToken()
            {
                GivenUserId(SomeUserId);
                GivenToken(null);
                GivenUserInDatabase(SomeUserId, AnotherToken, SomeLogInDateTime);
                GivenUserService();
                ThenExceptionIsThrown<UnauthorizedAccessException>(WhenGetUser, _exceptionMessageForMissingToken);
            }

            [Test]
            public void ShouldThrowUnauthorizedAccessExceptionWhenDifferentTokens()
            {
                GivenUserId(SomeUserId);
                GivenToken(SomeToken);
                GivenUserInDatabase(SomeUserId, AnotherToken, SomeLogInDateTime);
                GivenUserService();
                ThenExceptionIsThrown<UnauthorizedAccessException>(WhenGetUser, _exceptionMessageForDifferentTokens);
            }

            [Test]
            public void ShouldThrowUnauthorizedAccessExceptionWhenSessionExpired()
            {
                GivenUserId(SomeUserId);
                GivenToken(SomeToken);
                GivenUserInDatabase(SomeUserId, SomeToken, SomeLogInDateTime);
                GivenSystemClock(_aDateTime30MinAfterUserLogIn);
                GivenUserService();
                ThenExceptionIsThrown<UnauthorizedAccessException>(WhenGetUser, _exceptionMessageForExpiredSession);
            }

            private void GivenUserId(Guid userId)
            {
                _userId = userId;
            }

            private void GivenToken(string token)
            {
                _token = token;
            }

            private void GivenSystemClock(DateTime dateTime)
            {
                _systemClock.Setup(c => c.GetCurrentTime()).Returns(dateTime);
            }

            private void GivenUserInDatabase(Guid userId, string token, DateTime lastLogin)
            {
                var salt = Core.Helpers.Hashing.Salt.Create();
                var hash = Core.Helpers.Hashing.Hash.Create(token, salt);

                var _databaseWithUser = new List<User> { new User { Id = userId, Token = hash, Salt = salt, LastLoginOn = lastLogin } }.AsQueryable();

                _aDateTime30MinAfterUserLogIn = lastLogin.AddMinutes(30);
                _aDateTimeLessThan30MinAfterUserLogIn = lastLogin.AddMinutes(15);

                _userRepository.Setup(p => p.FindByCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(_databaseWithUser);
                _repositoryWrapper.Setup(p => p.User).Returns(_userRepository.Object);
            }

            private void GivenNoUserInDatabase()
            {
                var _databaseWithUser = new List<User>().AsQueryable();

                _userRepository.Setup(p => p.FindByCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(_databaseWithUser);
                _repositoryWrapper.Setup(p => p.User).Returns(_userRepository.Object);
            }


            private void GivenUserService()
            {
                _userService = new UserService(_repositoryWrapper.Object, _systemClock.Object);
            }

            private void WhenGetUser()
            {
                _foundUser = _userService.GetUser(_userId, _token);
            }

            private void ThenUserIsFound()
            {
                Assert.IsNotNull(_foundUser);
            }

            private readonly Guid SomeUserId = new Guid("8497c1be-5c95-4de2-a463-2703aa65e784");
            private readonly string SomeToken = "SomeToken";
            private readonly string AnotherToken = "AnotherToken";
            private readonly DateTime SomeLogInDateTime = new DateTime(2019, 6, 26);
            private readonly string _exceptionMessageForMissingToken = "Unauthorized";
            private readonly string _exceptionMessageForDifferentTokens = "Unauthorized";
            private readonly string _exceptionMessageForExpiredSession = "Invalid Session";

            private DateTime _aDateTime30MinAfterUserLogIn;
            private DateTime _aDateTimeLessThan30MinAfterUserLogIn;
            private IUserService _userService;
            private Mock<ISystemClock> _systemClock;
            private Mock<IUserRepository> _userRepository;
            private Mock<IRepositoryWrapper> _repositoryWrapper;
            private Guid _userId;
            private string _token;
            private UserDto _foundUser;
        }
    }
}
