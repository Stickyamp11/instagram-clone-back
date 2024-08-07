using Domain.Entities;
using FluentResults;
using FluentValidation;
using Instagram_Api.Application.Publications.Queries;
using MediatR;

namespace Instagram_Api.Application.Auth.Queries
{
    public class UserByUserEmailQueryValidator : AbstractValidator<UserByUserEmailQuery>
    {
        public UserByUserEmailQueryValidator()
        {
            RuleFor(x => x.userEmail).NotEmpty();
        }
    }
}
