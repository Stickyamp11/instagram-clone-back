using Application;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Errors;
using Instagram_Api.Application.Auth.Models;
using MediatR;

namespace Instagram_Api.Application.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this._jwtTokenGenerator = jwtTokenGenerator;
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<Result<AuthResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var request = command.request;

            if (_userRepository.GetUserByEmail(request.Email) is not null)
            {
                return Result.Fail<AuthResult>(new DuplicateEmailError());
            }

            Guid userGuid = Guid.NewGuid();

            User newUser = new User()
            {
                Guid = new UserGuid(userGuid),
                FullName = request.FullName,
                Email = request.Email,
                Password = _passwordHasher.HashPassword(request.Password)
            };
            _userRepository.Add(newUser);

            var token = _jwtTokenGenerator.GenerateToken(userGuid, request.Email);
            return new AuthResult(userGuid, token);
        }
    }
}
