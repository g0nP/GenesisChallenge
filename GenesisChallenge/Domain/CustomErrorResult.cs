using Microsoft.AspNetCore.Mvc;

namespace GenesisChallenge.Domain
{
    /// <summary>
    /// Creates a custom error result to be returned by controllers
    /// </summary>
    public class CustomErrorResult : JsonResult
    {
        public CustomErrorResult(int statusCode, string message)
            : base(new CustomError(message))
        {
            StatusCode = statusCode;
        }
    }
}
