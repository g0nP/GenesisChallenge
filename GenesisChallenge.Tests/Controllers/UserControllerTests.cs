using GenesisChallenge.Controllers;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GenesisChallenge.Tests.Controllers
{
    public class UserControllerTests : Test
    {
        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Test]
        public void ShouldReturnOkWhenGetUser()
        {
            GivenGetUserResponse();
            GivenUserController();
            WhenGetUser();
            ThenOkStatusCodeIsReturned();
            ThenCorrectTypeIsReturned<IUser>();
        }

        [Test]
        public void ShouldReturnNotFoundDuringGetUserWhenKeyNotFoundExceptionIsThrown()
        {
            GivenGetUserThrowsKeyNotFoundException();
            GivenUserController();
            WhenGetUser();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status404NotFound);
        }

        private void GivenGetUserResponse()
        {
            _user = new User();
            _userServiceMock.Setup(p => p.GetUser(_userId)).Returns(_user);
        }

        private void GivenGetUserThrowsKeyNotFoundException()
        {
            _userServiceMock.Setup(p => p.GetUser(_userId)).Throws(new KeyNotFoundException());
        }

        private void GivenUserController()
        {
            _userController = new UserController(_userServiceMock.Object);
        }

        private void WhenGetUser()
        {
            _actionResult = _userController.GetUser(_userId);
        }

        private void ThenCorrectTypeIsReturned<T>()
        {
            var result = _actionResult as ObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T>(result.Value);
        }

        private void ThenOkStatusCodeIsReturned()
        {
            var result = _actionResult as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }

        private void ThenErrorStatusCodeIsReturned(int statusCode)
        {
            var result = _actionResult as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, statusCode);
        }

        private static DateTime[] DateTimeValue = new DateTime[] { DateTime.UtcNow };

        private readonly Guid _userId = Guid.NewGuid();

        private UserController _userController;
        private Mock<IUserService> _userServiceMock;
        private IUser _user;
        private IActionResult _actionResult;
    }
}
