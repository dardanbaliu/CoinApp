using CoinApp.Domain.SeedWork;
using CoinApp.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Domain.Coins
{
    public class Coin : Entity
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
