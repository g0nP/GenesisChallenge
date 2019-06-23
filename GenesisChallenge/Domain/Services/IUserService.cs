using GenesisChallenge.Domain.Models;
using GenesisChallenge.Dtos;
using GenesisChallenge.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenesisChallenge.Domain.Services
{
    public interface IUserService
    {
        ISignUpResponse SignUp(SignUpDto user);
        ISignInResponse SignIn(SignInDto signInDto);
        IUser GetUser(Guid userId);
    }
}
