using FluentValidation;

namespace Instagram_Api.Application.Auth.Commands
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() 
        {
            RuleFor(x => x.request.FullName).NotEmpty();
            RuleFor(x => x.request.Email).NotEmpty();
            RuleFor(x => x.request.Password).NotEmpty();
        }
    }
}
