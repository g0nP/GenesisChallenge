using GenesisChallenge.Controllers;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using static GenesisChallenge.Domain.CustomExceptions;

namespace GenesisChallenge.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
        }

        [Test]
        public void ShouldReturnOkWhenSignUp()
        {
            GivenSignUpResponse();
            GivenUserController();
            WhenSignUp();
            ThenOkStatusCodeIsReturned();
            ThenCorrectTypeIsReturned<SignUpResponse>();
        }

        [Test]
        public void ShouldReturnBadRequestDuringSingUpWhenArgumentNullExceptionIsThrown()
        {
            GivenSignUpThrowsArgumentNullException();
            GivenUserController();
            WhenSignUp();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status400BadRequest);
        }

        [Test]
        public void ShouldReturnConflictDuringSingUpWhenEmailAlreadyExistsExceptionIsThrown()
        {
            GivenSignUpThrowsEmailAlreadyExistsException();
            GivenUserController();
            WhenSignUp();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status409Conflict);
        }

        [Test]
        public void ShouldReturnInternalServerErrorDuringSingUpWhenExceptionIsThrown()
        {
            GivenSignUpThrowsException();
            GivenUserController();
            WhenSignUp();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status500InternalServerError);
        }

        [Test]
        public void ShouldReturnOkWhenSignIn()
        {
            GivenSignInResponse();
            GivenUserController();
            WhenSignIn();
            ThenOkStatusCodeIsReturned();
            ThenCorrectTypeIsReturned<SignInResponse>();
        }

        [Test]
        public void ShouldReturnBadRequestDuringSingInWhenArgumentNullExceptionIsThrown()
        {
            GivenSignInThrowsArgumentNullException();
            GivenUserController();
            WhenSignIn();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status400BadRequest);
        }

        [Test]
        public void ShouldReturnConflictDuringSingInWhenInexistentEmailExceptionIsThrown()
        {
            GivenSignInThrowsInexistentEmailException();
            GivenUserController();
            WhenSignIn();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status404NotFound);
        }

        [Test]
        public void ShouldReturnConflictDuringSingInWhenInvalidPasswordExceptionIsThrown()
        {
            GivenSignInThrowsInvalidPasswordException();
            GivenUserController();
            WhenSignIn();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status401Unauthorized);
        }

        [Test]
        public void ShouldReturnInternalServerErrorDuringSingInWhenExceptionIsThrown()
        {
            GivenSignInThrowsException();
            GivenUserController();
            WhenSignIn();
            ThenErrorStatusCodeIsReturned(StatusCodes.Status500InternalServerError);
        }

        private void GivenSignUpResponse()
        {
            _signUpResponse = new SignUpResponse();
            _authenticationServiceMock.Setup(p => p.SignUp(_signUpDto)).Returns(_signUpResponse);
        }

        private void GivenSignUpThrowsArgumentNullException()
        {
            _authenticationServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new ArgumentNullException());
        }

        private void GivenSignUpThrowsEmailAlreadyExistsException()
        {
            _authenticationServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new EmailAlreadyExistsException());
        }

        private void GivenSignUpThrowsException()
        {
            _authenticationServiceMock.Setup(p => p.SignUp(_signUpDto)).Throws(new Exception());
        }

        private void GivenSignInResponse()
        {
            _signInResponse = new SignInResponse();
            _authenticationServiceMock.Setup(p => p.SignIn(_signInDto)).Returns(_signInResponse);
        }

        private void GivenSignInThrowsArgumentNullException()
        {
            _authenticationServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new ArgumentNullException());
        }

        private void GivenSignInThrowsInexistentEmailException()
        {
            _authenticationServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new InexistentEmailException());
        }

        private void GivenSignInThrowsInvalidPasswordException()
        {
            _authenticationServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new InvalidPasswordException());
        }

        private void GivenSignInThrowsException()
        {
            _authenticationServiceMock.Setup(p => p.SignIn(_signInDto)).Throws(new Exception());
        }

        private void GivenUserController()
        {
            _authenticationController = new AuthenticationController(_authenticationServiceMock.Object);
        }

        private void WhenSignUp()
        {
            _actionResult = _authenticationController.SignUp(_signUpDto);
        }

        private void WhenSignIn()
        {
            _actionResult = _authenticationController.SignIn(_signInDto);
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

        private readonly SignUpDto _signUpDto = new SignUpDto();
        private readonly SignInDto _signInDto = new SignInDto();

        private AuthenticationController _authenticationController;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private ISignUpResponse _signUpResponse;
        private ISignInResponse _signInResponse;
        private IActionResult _actionResult;
    }
}
