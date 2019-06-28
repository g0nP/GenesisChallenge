using GenesisChallenge.Core.Dtos;
using GenesisChallenge.Core.Responses;

namespace GenesisChallenge.Abstractions.Services
{
    /// <summary>
    /// Service for authenticating users
    /// </summary>
    /// <remarks>
    /// Allows signin up and signin in
    /// </remarks>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="signUpDto">Necessary information to create a new User</param>
        ISignUpResponse SignUp(SignUpDto signUpDto);

        /// <summary>
        /// Logs a user in
        /// </summary>
        /// <param name="signInDto">Necessary information to log a User in</param>
        ISignInResponse SignIn(SignInDto signInDto);
    }
}
