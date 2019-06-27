using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenesisChallenge.Controllers
{
    /// <summary>
    /// Handles request to unexistent routes
    /// </summary>
    /// <remarks>
    /// Send customs errors in json format
    /// </remarks>
    [AllowAnonymous]
    [Produces("application/json")]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Handles requests to unexistent resources
        /// </summary>
        [HttpGet("error/404")]
        public JsonResult Error404()
        {
            return new JsonResult("404 - The resource was not found");
        }

        /// <summary>
        /// Handles any other type of request error
        /// </summary>
        [HttpGet("error/{code:int}")]
        public JsonResult Error(int code)
        {
            return new JsonResult($"Error code {code}");
        }
    }
}