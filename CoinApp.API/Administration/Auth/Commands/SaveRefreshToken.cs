using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.Auth.Commands
{
    public class SaveRefreshToken : IRequest<bool>
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }
        public SaveRefreshToken(string username, string refreshToken)
        {
            Username = username;
            RefreshToken = refreshToken;
        }
    }
}
