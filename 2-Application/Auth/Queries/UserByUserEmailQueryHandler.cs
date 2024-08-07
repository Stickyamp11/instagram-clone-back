using Application;
using Domain.Entities;
using FluentResults;
using Instagram_Api.Application.Auth.Errors;
using Instagram_Api.Application.Auth.Models;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram_Api.Application.Auth.Queries
{
    public class UserByUserEmailQueryHandler : IRequestHandler<UserByUserEmailQuery, Result<User>>
    {
        private readonly IUserRepository _userRepository;

        public UserByUserEmailQueryHandler(
            IUserRepository _userRepository)
        {
            this._userRepository = _userRepository;
        }

        public async Task<Result<User>> Handle(UserByUserEmailQuery query, CancellationToken cancellationToken)
        {
            if (_userRepository.GetUserByEmail(query.userEmail) is not User user)
            {
                return Result.Fail<User>(new UserDoesNotExistError());
            }

            return _userRepository.GetUserByEmail(query.userEmail);
        }
    }
}
