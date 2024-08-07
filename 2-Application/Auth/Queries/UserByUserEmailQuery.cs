using Domain.Entities;
using FluentResults;
using MediatR;

namespace Instagram_Api.Application.Auth.Queries
{
    public record UserByUserEmailQuery(
       string userEmail) : IRequest<Result<User>>;
}
