using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;

namespace GenesisChallenge.Domain.Services
{
    public interface IAuthenticationService
    {
        ISignUpResponse SignUp(SignUpDto user);
        ISignInResponse SignIn(SignInDto signInDto);
    }
}
