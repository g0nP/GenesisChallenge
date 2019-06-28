using System;

namespace GenesisChallenge.Core.Responses
{
    /// <summary>
    /// Encapsulates the data for the Sign In response
    /// </summary>
    public interface ISignInResponse
    {
        DateTime CreationOn { get; set; }
        Guid Id { get; set; }
        DateTime LastLoginOn { get; set; }
        DateTime LastUpdatedOn { get; set; }
        string Token { get; set; }
    }

    /// <summary>
    /// Implements ISignInResponse
    /// </summary>
    public class SignInResponse : ISignInResponse
    {
        public Guid Id { get; set; }
        public DateTime CreationOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public DateTime LastLoginOn { get; set; }
        public string Token { get; set; }
    }
}
