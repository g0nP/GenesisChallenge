using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GenesisChallenge.Tests.Services
{
    public class UserServiceTests
    {
        public class Tests : Test
        {
            [SetUp]
            public void Setup()
            {
                _userService = new UserService();
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCaseSource(nameof(RegisteredUserId))]
            public void ShouldReturnUserWhenGetUser(Guid userId)
            {
                GivenUserId(userId);
                WhenGetUser();
                ThenUserIsFound();
            }

            [Ignore("Until persistence layer is created")]
            [Test]
            [TestCaseSource(nameof(NotRegisteredUserId))]
            public void ShouldThrowKeyNotFoundExceptionWhenUserDoesntExists(Guid userId)
            {
                GivenUserId(userId);
                ThenExceptionIsThrown<KeyNotFoundException>(WhenGetUser);
            }

            private void GivenUserId(Guid userId)
            {
                _userId = userId;
            }

            private void WhenGetUser()
            {
                _foundUser = _userService.GetUser(_userId);
            }

            private void ThenUserIsFound()
            {
                Assert.IsNotNull(_foundUser);
                Assert.That(_foundUser.Id == _userId);
            }

            private static Guid[] RegisteredUserId = new Guid[] { new Guid("0700c1be-5c95-4de2-a463-2703aa65c480") };
            private static Guid[] NotRegisteredUserId = new Guid[] { new Guid("8497c1be-5c95-4de2-a463-2703aa65e784") };

            private IUserService _userService;
            private Guid _userId;
            private IUser _foundUser;
        }
    }
}
