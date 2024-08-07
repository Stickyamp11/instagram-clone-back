using Application;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Errors;
using Instagram_Api.Application.Auth.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this._jwtTokenGenerator = jwtTokenGenerator;
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<Result<AuthResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var request = command.request;

            if (_userRepository.GetUserByEmail(request.Email) is not User user)
            {
                return Result.Fail<AuthResult>(new PasswordMatchError());
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                return Result.Fail<AuthResult>(new PasswordMatchError());
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Guid.Value, request.Email);
            return new AuthResult(user.Guid.Value, token);
        }
    }
}
