using CoinApp.Domain.Coins;
using CoinApp.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Coins.Commands
{
    public class SaveFavoriteCoinsHandler : IRequestHandler<SaveFavoriteCoins, bool>
    {
        private readonly IRepository<Coin> _iCoinRepository;

        public SaveFavoriteCoinsHandler(IRepository<Coin> iCoinRepository)
        {
            _iCoinRepository = iCoinRepository;
        }
        public async Task<bool> Handle(SaveFavoriteCoins request, CancellationToken cancellationToken)
        {
            var favCoins = _iCoinRepository.ListByCriteria(t => t.UserId == request.UserId);
            var favCoinsToBeDeleted = new List<Coin>();
            var favCoinsToBeAdded = new List<Coin>();
            if (favCoins.Count() > 0)
            {
                favCoinsToBeDeleted.AddRange(favCoins.Where(t => !request.CoinIds.Contains(t.Id)));
                favCoinsToBeAdded.AddRange(request.CoinIds.Where(r => !favCoins.Select(t => t.Id).Contains(r)).Select(coin => new Coin
                {
                    Id = coin,
                    UserId = request.UserId
                }));
            }
            else
            {
                favCoinsToBeAdded.AddRange(request.CoinIds.Select(coin => new Coin
                {
                    Id = coin,
                    UserId = request.UserId
                }));
            }
            if (favCoinsToBeAdded.Count() > 0)
                await _iCoinRepository.AddRangeAsync(favCoinsToBeAdded);
            if (favCoinsToBeDeleted.Count() > 0)
                await _iCoinRepository.DeleteRangeAsync(favCoinsToBeDeleted);
            return Task.FromResult(true).Result;
        }
    }
}
