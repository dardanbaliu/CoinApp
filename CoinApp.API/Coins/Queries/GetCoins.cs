using CoinApp.SharedModel.Coins;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Coins.Queries
{
    public class GetCoins:IRequest<List<CoinDto>>
    {
    }
}
