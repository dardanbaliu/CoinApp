using CoinApp.API.Helper;
using CoinApp.Domain.Coins;
using CoinApp.Infrastructure.Repositories;
using CoinApp.SharedModel.Coins;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.API.Coins.Queries
{
    public class GetFavoriteCoinsHandler : IRequestHandler<GetFavoriteCoins, List<CoinDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Coin> _iCoinRepository;

        public GetFavoriteCoinsHandler(IConfiguration configuration, IRepository<Coin> iCoinRepository)
        {
            _configuration = configuration;
            _iCoinRepository = iCoinRepository;
        }
        public async Task<List<CoinDto>> Handle(GetFavoriteCoins request, CancellationToken cancellationToken)
        {
            var link = _configuration["CoinDataLink"];
            var model = new List<CoinDto>();
            var userCoins = _iCoinRepository.ListByCriteria(t => t.UserId == request.UserId);
            var result = await HttpHelper.GetWithLink(link);
            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<RequestCoinDto>(response);
                model = responseData.Data.Where(t=>userCoins.Select(r=>r.Id).Contains(t.Id)).ToList();
            }
            return Task.FromResult(model).Result;
        }
    }
}
