using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Commands;
using Instagram_Api.Application.Auth.Models;
using Instagram_Api.Application.Publications.Commands;
using Instagram_Api.Application.Publications.Queries;
using MediatR;

namespace Instagram_Api.Application.Common
{
    public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(RegisterCommand request, RequestHandlerDelegate<Result<AuthResult>> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }
    public class ValidateLoginCommandBehavior : IPipelineBehavior<LoginCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(LoginCommand request, RequestHandlerDelegate<Result<AuthResult>> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class ValidatePublicationsByUserGuidQueryBehavior : IPipelineBehavior<PublicationsByUserGuidQuery, Result<List<Publication>>>
    {
        public async Task<Result<List<Publication>>> Handle(PublicationsByUserGuidQuery request, RequestHandlerDelegate<Result<List<Publication>>> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class ValidateAddPublicationCommandBehavior : IPipelineBehavior<AddPublicationCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AddPublicationCommand request, RequestHandlerDelegate<Result<int>> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }
}
