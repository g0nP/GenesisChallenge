using GenesisChallenge.Domain.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using GenesisChallenge.Controllers;
using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using static GenesisChallenge.Domain.CustomExceptions;
using GenesisChallenge.Domain.Models;

namespace GenesisChallenge.Tests.Controllers
{
    public class UserControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Test]
        public void ShouldReturnOkWhenSignUp()
        {
            GivenSignUpResponse();
            GivenUserController();
            WhenSignUp();
            ThenOkIsReturned<SignUpResponse>();
        }

        [Test]
        public void ShouldReturnBadRequestDuringSingUpWhenArgumentNullExceptionIsThrown()
        {
            GivenSignUpThrowsArgumentNullException();
            GivenUserController();
            WhenSignUp();
            ThenBadRequestIsReturned();
        }

        [Test]
        public void ShouldReturnConflictDuringSingUpWhenEmailAlreadyExistsExceptionIsThrown()
        {
            GivenSignUpThrowsEmailAlreadyExistsException();
            GivenUserController();
            WhenSignUp();
            ThenConflictIsReturned();
        }

        [Test]
        public void ShouldReturnInternalServerErrorDuringSingUpWhenExceptionIsThrown()
        {
            GivenSignUpThrowsException();
            GivenUserController();
            WhenSignUp();
            ThenInternalServerErrorIsReturned();
        }

        [Test]
        public void ShouldReturnOkWhenSignIn()
        {
            GivenSignInResponse();
            GivenUserController();
            WhenSignIn();
            ThenOkIsReturned<SignInResponse>();
        }

        [Test]
        public void ShouldReturnBadRequestDuringSingInWhenArgumentNullExceptionIsThrown()
        {
            GivenSignInThrowsArgumentNullException();
            GivenUserController();
            WhenSignIn();
            ThenBadRequestIsReturned();
        }

        [Test]
        public void ShouldReturnConflictDuringSingInWhenInexistentEmailExceptionIsThrown()
        {
            GivenSignInThrowsInexistentEmailException();
            GivenUserController();
            WhenSignIn();
            ThenNotFoundIsReturned();
        }

        [Test]
        public void ShouldReturnConflictDuringSingInWhenInvalidPasswordExceptionIsThrown()
        {
            GivenSignInThrowsInvalidPasswordException();
            GivenUserController();
            WhenSignIn();
            ThenUnauthorizedIsReturned();
        }

        [Test]
        public void ShouldReturnInternalServerErrorDuringSingInWhenExceptionIsThrown()
        {
            GivenSignInThrowsException();
            GivenUserController();
            WhenSignIn();
            ThenInternalServerErrorIsReturned();
        }

        [Test]
        public void ShouldReturnOkWhenGetUser()
        {
            GivenGetUserResponse();
            GivenUserController();
            WhenGetUser();
            ThenOkIsReturned<IUser>();
        }

        [Test]
        public void ShouldReturnNotFoundDuringGetUserWhenKeyNotFoundExceptionIsThrown()
        {
            GivenSignInThrowsKeyNotFoundException();
            GivenUserController();
            WhenGetUser();
            ThenNotFoundIsReturned();
        }

        private void GivenSignUpResponse()
        {
            _signUpResponse = new SignUpResponse();
            _userServiceMock.Setup(p => p.SignUp(_signUpDto)).Returns(_signUpResponse);
        }

        private void GivenSignUpThrowsArgumentNullException()
        {
            _userServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new ArgumentNullException());
        }

        private void GivenSignUpThrowsEmailAlreadyExistsException()
        {
            _userServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new EmailAlreadyExistsException());
        }

        private void GivenSignUpThrowsException()
        {
            _userServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new Exception());
        }

        private void GivenSignInResponse()
        {
            _signInResponse = new SignInResponse();
            _userServiceMock.Setup(p => p.SignIn(_signInDto)).Returns(_signInResponse);
        }

        private void GivenSignInThrowsArgumentNullException()
        {
            _userServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new ArgumentNullException());
        }

        private void GivenSignInThrowsInexistentEmailException()
        {
            _userServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new InexistentEmailException());
        }

        private void GivenSignInThrowsInvalidPasswordException()
        {
            _userServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new InvalidPasswordException());
        }

        private void GivenSignInThrowsException()
        {
            _userServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new Exception());
        }

        private void GivenGetUserResponse()
        {
            _user = new User();
            _userServiceMock.Setup(p => p.GetUser(_userId)).Returns(_user);
        }

        private void GivenSignInThrowsKeyNotFoundException()
        {
            _userServiceMock.Setup(p => p.GetUser(_userId)).Throws(new KeyNotFoundException());
        }

        private void GivenUserController()
        {
            _userController = new UserController(_userServiceMock.Object);
        }

        private void WhenSignUp()
        {
            _actionResult = _userController.SignUp(_signUpDto);
        }

        private void WhenSignIn()
        {
            _actionResult = _userController.SignIn(_signInDto);
        }

        private void WhenGetUser()
        {
            _actionResult = _userController.GetUser(_userId);
        }

        private void ThenOkIsReturned<T>()
        {
            var result = _actionResult as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            Assert.IsInstanceOf<T>(result.Value);
        }

        private void ThenBadRequestIsReturned()
        {
            var result = _actionResult as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        private void ThenConflictIsReturned()
        {
            var result = _actionResult as ConflictObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status409Conflict);
        }

        private void ThenNotFoundIsReturned()
        {
            var result = _actionResult as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status404NotFound);
        }

        private void ThenUnauthorizedIsReturned()
        {
            var result = _actionResult as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status401Unauthorized);
        }

        private void ThenInternalServerErrorIsReturned()
        {
            var result = _actionResult as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status500InternalServerError);
        }

        private static DateTime[] DateTimeValue = new DateTime[] { DateTime.UtcNow };

        private readonly SignUpDto _signUpDto = new SignUpDto();
        private readonly SignInDto _signInDto = new SignInDto();
        private readonly Guid _userId = Guid.NewGuid();

        private UserController _userController;
        private Mock<IUserService> _userServiceMock;
        private ISignUpResponse _signUpResponse;
        private ISignInResponse _signInResponse;
        private IUser _user;
        private IActionResult _actionResult;
    }
}
