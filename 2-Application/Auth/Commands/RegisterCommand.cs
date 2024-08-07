using FluentResults;
using Instagram_Api.Application.Auth.Models;
using MediatR;

namespace Instagram_Api.Application.Auth.Commands
{
    public record RegisterCommand(
        RegisterRequest request) : IRequest<Result<AuthResult>>;
}
