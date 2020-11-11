using CoinApp.API.Administration.DomainServices;
using CoinApp.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Users.Commands
{
    public class LoginHandler : IRequestHandler<Login, User>
    {
        private readonly IAuthentication _authentication;

        public LoginHandler(IAuthentication authentication)
        {

            _authentication = authentication;
        }

        public Task<User> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _authentication.Authorize(request.Username, request.Password);
                return Task.FromResult(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}