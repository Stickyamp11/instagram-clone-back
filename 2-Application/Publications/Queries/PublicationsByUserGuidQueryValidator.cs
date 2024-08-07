using FluentValidation;

namespace Instagram_Api.Application.Publications.Queries
{
    public class PublicationsByUserGuidQueryValidator : AbstractValidator<PublicationsByUserGuidQuery>
    {
        public PublicationsByUserGuidQueryValidator()
        {
            RuleFor(x => x.userGuid).NotEmpty();
            RuleFor(x => x.range).NotEmpty();
        }
    }
}
