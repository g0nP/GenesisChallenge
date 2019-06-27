using GenesisChallenge.Domain;
using GenesisChallenge.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GenesisChallenge.Controllers
{
    /// <summary>
    /// Manages User actions
    /// </summary>
    /// <remarks>
    /// Provides an endpoint for searching for a user
    /// </remarks>
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a user by its id
        /// </summary>
        /// <returns>User or error message</returns>
        /// <param name="id">The user id to search</param>
        /// <response code="200">The user was found</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                var accessToken = User.FindFirst("access_token")?.Value;

                var user = _userService.GetUser(id, accessToken);
                return Ok(user);

            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new CustomErrorResult(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
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