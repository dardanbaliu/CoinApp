using CoinApp.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Coins.Commands
{
    public class SaveFavoriteCoins : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public List<string> CoinIds { get; set; }

        public SaveFavoriteCoins(List<string> coinIds, Guid userId)
        {
            CoinIds = coinIds;
            UserId = userId;
        }
    }
}
