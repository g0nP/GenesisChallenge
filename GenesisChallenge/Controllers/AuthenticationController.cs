using GenesisChallenge.Abstractions.Services;
using GenesisChallenge.Core;
using GenesisChallenge.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static GenesisChallenge.Core.CustomExceptions;

namespace GenesisChallenge.Controllers
{
    /// <summary>
    /// Manages User authentication
    /// </summary>
    /// <remarks>
    /// Provides endpoints for signing up and signing in
    /// </remarks>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>Created user or error message</returns>
        /// <response code="200">The user was created</response>
        /// <response code="400">An argument was not provided</response>
        /// <response code="409">Email already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto value)
        {
            try
            {

                var signUpResponse = await _authenticationService.SignUp(value);
                return Ok(signUpResponse);

            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (EmailAlreadyExistsException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Logs the user in
        /// </summary>
        /// <returns>User or error message</returns>
        /// <response code="200">The user was signed in</response>
        /// <response code="400">An argument was not provided</response>
        /// <response code="404">User not found</response>
        /// <response code="401">User not authorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto value)
        {
            try
            {

                var signInResponse = await _authenticationService.SignIn(value);
                return Ok(signInResponse);

            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (UnexistentEmailException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}