using GenesisChallenge.Controllers;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;

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
            GivenToken("SomeToken");
            GivenUserController();
            WhenGetUser();
            ThenOkStatusCodeIsReturned();
            ThenCorrectTypeIsReturned<IUser>();
        }

        [Test]
        public void ShouldReturnNotFoundDuringGetUserWhenKeyNotFoundExceptionIsThrown()
        {
            GivenGetUserThrowsKeyNotFoundException();
            GivenToken("SomeToken");
            GivenUserController();
            WhenGetUser();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status404NotFound);
        }

        [Test]
        public void ShouldReturnUnauthorizedDuringGetUserWhenUnauthorizedAccessExceptionIsThrown()
        {
            GivenGetUserThrowsUnauthorizedAccessException();
            GivenToken("SomeToken");
            GivenUserController();
            WhenGetUser();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status401Unauthorized);
        }

        [Test]
        public void ShouldReturnInternalServerErrorDuringGetUserWhenExceptionIsThrown()
        {
            GivenGetUserThrowsException();
            GivenToken("SomeToken");
            GivenUserController();
            WhenGetUser();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status500InternalServerError);
        }

        private void GivenGetUserResponse()
        {
            _user = new User();
            _userServiceMock.Setup(p => p.GetUser(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_user);
        }

        private void GivenToken(string token)
        {
            _token = token;
        }

        private void GivenGetUserThrowsKeyNotFoundException()
        {
            _userServiceMock.Setup(p => p.GetUser(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new KeyNotFoundException());
        }

        private void GivenGetUserThrowsUnauthorizedAccessException()
        {
            _userServiceMock.Setup(p => p.GetUser(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new UnauthorizedAccessException());
        }

        private void GivenGetUserThrowsException()
        {
            _userServiceMock.Setup(p => p.GetUser(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new Exception());
        }

        private void GivenUserController()
        {
            _userController = new UserController(_userServiceMock.Object);

            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            var userMock = new Mock<ClaimsPrincipal>();

            var claim = new Claim("acces_token", _token);
            userMock.Setup(p => p.FindFirst(It.IsAny<string>())).Returns(claim);

            mockContext.SetupGet(hc => hc.User).Returns(userMock.Object);
            _userController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
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

        private readonly Guid _userId = new Guid("8497c1be-5c95-4de2-a463-2703aa65e784");

        private UserController _userController;
        private Mock<IUserService> _userServiceMock;
        private IUser _user;
        private string _token;
        private IActionResult _actionResult;
    }
}
