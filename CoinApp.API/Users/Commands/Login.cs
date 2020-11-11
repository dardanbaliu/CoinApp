using CoinApp.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Users.Commands
{
    public class Login : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Login(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}