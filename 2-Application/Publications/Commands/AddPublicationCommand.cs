using Domain.Entities;
using FluentResults;
using MediatR;

namespace Instagram_Api.Application.Publications.Commands
{
    public record AddPublicationCommand(
        Publication request, string userGuid) : IRequest<Result<int>>;
}