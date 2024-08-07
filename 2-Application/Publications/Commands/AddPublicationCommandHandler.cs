using Application;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Errors;
using Instagram_Api.Application.Auth.Models;
using MediatR;

namespace Instagram_Api.Application.Publications.Commands
{
    public class AddPublicationCommandHandler : IRequestHandler<AddPublicationCommand, Result<int>>
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly IUserRepository _userRepository;


        public AddPublicationCommandHandler(IUserRepository userRepository, IPublicationRepository publicationRepository)
        {
            _publicationRepository = publicationRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<int>> Handle(AddPublicationCommand command, CancellationToken cancellationToken)
        {
            Publication request = command.request;
            string userGuid = command.userGuid;

            var user = _userRepository.GetUserByGuid(userGuid);
            if (_userRepository.GetUserByGuid(userGuid) is null)
            {
                return Result.Fail<int>(new UserDoesNotExistError());
            }

            request.userId = user.Id;

            _publicationRepository.Add(request);

            return 1;
        }
    }
}
