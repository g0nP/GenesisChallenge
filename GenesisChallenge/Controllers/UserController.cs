using GenesisChallenge.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GenesisChallenge.Controllers
{
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
        /// <response code="200">The user was found</response>
        /// <response code="404">User not found</response>
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
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}