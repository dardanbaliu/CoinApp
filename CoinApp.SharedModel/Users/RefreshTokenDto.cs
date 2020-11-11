using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.SharedModel.Users
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
