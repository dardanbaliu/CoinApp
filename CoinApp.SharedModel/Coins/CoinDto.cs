using System.Collections.Generic;

namespace CoinApp.SharedModel.Coins
{
    public class CoinDto
    {
        public string Id { get; set; }
        public string Rank { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Supply { get; set; }
        public string MaxSupply { get; set; }
    }

    public class RequestCoinDto
    {
        public string Timestamp { get; set; }
        public List<CoinDto> Data { get; set; }
    }
}