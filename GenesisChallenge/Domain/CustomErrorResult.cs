using Microsoft.AspNetCore.Mvc;

namespace GenesisChallenge.Domain
{
    public class CustomErrorResult : JsonResult
    {
        public CustomErrorResult(int statusCode, string message)
            : base(new CustomError(message))
        {
            StatusCode = statusCode;
        }
    }
}
