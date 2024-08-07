using Domain.Entities;
using FluentResults;
using FluentValidation;
using Instagram_Api.Application.Publications.Queries;
using MediatR;

namespace Instagram_Api.Application.Publications.Commands
{
    public class AddPublicationCommandValidator : AbstractValidator<AddPublicationCommand>
    {
        public AddPublicationCommandValidator()
        {
            RuleFor(x => x.request.Title).NotEmpty();
            RuleFor(x => x.request.Description).NotEmpty();
        }
    }
}