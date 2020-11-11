using CoinApp.Domain.Users;
using CoinApp.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.Auth.Commands
{
    public class SaveRefreshTokenHandler : IRequestHandler<SaveRefreshToken, bool>
    {
        private readonly IRepository<User> _iUserRepository;

        public SaveRefreshTokenHandler(IRepository<User> iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }
        public async Task<bool> Handle(SaveRefreshToken request, CancellationToken cancellationToken)
        {
            var user = _iUserRepository.GetSingleByCriteria(t => t.Username == request.Username);
            user.RefreshToken = request.RefreshToken;
            await _iUserRepository.UpdateAsync(user);
            return Task.FromResult(true).Result;
        }
    }
}
