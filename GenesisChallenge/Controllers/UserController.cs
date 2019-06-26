using GenesisChallenge.Domain;
using GenesisChallenge.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GenesisChallenge.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

                var user = _userService.GetUser(id);
                return Ok(user);

            }
            catch (KeyNotFoundException ex)
            {
                return new CustomErrorResult(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return new CustomErrorResult(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}