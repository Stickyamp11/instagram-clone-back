using Application;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Errors;
using Instagram_Api.Application.Auth.Models;
using Instagram_Api.Application.Publications.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Publications.Queries
{
    public class PublicationsByUserGuidQueryHandler : IRequestHandler<PublicationsByUserGuidQuery, Result<PaginatedList<Publication>>>
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly IUserRepository _userRepository;

        private const int MAX_PAGE_SIZE = 20;

        public PublicationsByUserGuidQueryHandler(
            IPublicationRepository _publicationRepository,
            IUserRepository _userRepository)
        {
            this._publicationRepository = _publicationRepository;
            this._userRepository = _userRepository;
        }

        public async Task<Result<PaginatedList<Publication>>> Handle(PublicationsByUserGuidQuery query, CancellationToken cancellationToken)
        {
            if (_userRepository.GetUserByGuid(query.userGuid) is not User user)
            {
                return Result.Fail<PaginatedList<Publication>>(new UserDoesNotExistError());
            }

            string[] ranges = query.range.Split("-");

            if (!int.TryParse(ranges[0], out int range_low) || !int.TryParse(ranges[1], out int range_max))
            {
                return Result.Fail<PaginatedList<Publication>>(new UserDoesNotExistError());
            }

            int pageSize = range_max - range_low;

            if(pageSize > MAX_PAGE_SIZE)
            {
                return Result.Fail<PaginatedList<Publication>>(new UserDoesNotExistError());
            }

            var publications = _publicationRepository.GetPublicationsByUserGuid(query.userGuid, range_low, range_max);
            var totalPublications = await _publicationRepository.GetTotalPublicationsByUserGuid(query.userGuid);

            return new PaginatedList<Publication>(publications, range_low, range_max, pageSize, totalPublications);
        }
    }
}
