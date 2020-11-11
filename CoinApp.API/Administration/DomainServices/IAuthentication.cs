using CoinApp.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinApp.API.Administration.DomainServices
{
    public interface IAuthentication
    {
        User Authorize(string username, string password);
    }
}