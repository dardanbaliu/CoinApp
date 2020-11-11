using CoinApp.Domain.Users;
using CoinApp.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.Auth.Queries
{
    public class GetRefreshTokenHandler : IRequestHandler<GetRefreshToken, string>
    {
        private readonly IRepository<User> _iUserRepository;

        public GetRefreshTokenHandler(IRepository<User> iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }
        public Task<string> Handle(GetRefreshToken request, CancellationToken cancellationToken)
        {
            var user = _iUserRepository.GetSingleByCriteria(t => t.Username == request.Username);
            return Task.FromResult(user.RefreshToken);
        }
    }
}
