using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Domain.Interfaces
{
    public interface IWorkContext
    {
        string UserId { get; }
        string BasePath { get; }
    }
}