using FluentValidation;

namespace Instagram_Api.Application.Auth.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() 
        {
            RuleFor(x => x.request.Email).NotEmpty();
            RuleFor(x => x.request.Password).NotEmpty();
        }
    }
}
