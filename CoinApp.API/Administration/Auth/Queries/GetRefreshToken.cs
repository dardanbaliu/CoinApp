using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.Auth.Queries
{
    public class GetRefreshToken : IRequest<string>
    {
        public string Username { get; set; }
        public GetRefreshToken(string username)
        {
            Username = username;
        }
    }
}
