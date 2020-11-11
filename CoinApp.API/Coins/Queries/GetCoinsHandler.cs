using CoinApp.API.Helper;
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
    public class GetCoinsHandler : IRequestHandler<GetCoins, List<CoinDto>>
    {
        private readonly IConfiguration _configuration;

        public GetCoinsHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<CoinDto>> Handle(GetCoins request, CancellationToken cancellationToken)
        {
            var link = _configuration["CoinDataLink"];
            var model = new List<CoinDto>();
            var result = await HttpHelper.GetWithLink(link);
            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<RequestCoinDto>(response);
                model = responseData.Data;
            }
            return Task.FromResult(model).Result;
        }
    }
}
