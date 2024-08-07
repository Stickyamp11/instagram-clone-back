using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Publications.Models;
using MediatR;

namespace Instagram_Api.Application.Publications.Queries
{
    public record PublicationsByUserGuidQuery(
       string userGuid,
       string range) : IRequest<Result<PaginatedList<Publication>>>;
}
