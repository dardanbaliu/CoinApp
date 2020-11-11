using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Infrastructure.Caching
{
    public interface ICacheStoreItem
    {
        string CacheKey { get; }
    }
}
