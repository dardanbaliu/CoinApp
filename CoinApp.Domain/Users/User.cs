using CoinApp.Domain.Coins;
using CoinApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Domain.Users
{
    public class User : Entity
    {
        public User()
        {
            Coins = new HashSet<Coin>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public virtual ICollection<Coin> Coins { get; set; }
    }
}
