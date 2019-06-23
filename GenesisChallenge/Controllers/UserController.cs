using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenesisChallenge.Domain.Models;
using GenesisChallenge.Domain.Services;
using GenesisChallenge.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GenesisChallenge.Domain.CustomExceptions;

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

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>Created user or error message</returns>
        /// <response code="200">The user was created</response>
        /// <response code="400">An argument was not provided</response>
        /// <response code="409">Email already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("/signup")]
        public IActionResult SignUp([FromBody] SignUpDto value)
        {
            try
            {

                var signUpResponse = _userService.SignUp(value);
                return Ok(signUpResponse);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
        public IActionResult SignIn([FromBody] SignInDto value)
        {
            try {

                var signInResponse = _userService.SignIn(value);
                return Ok(signInResponse);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InexistentEmailException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}